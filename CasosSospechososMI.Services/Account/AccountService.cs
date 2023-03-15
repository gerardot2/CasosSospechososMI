using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Account.Enums;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Configuration;
using CasosSospechososMI.Domain.Configuration.Interfaces;
using CasosSospechososMI.Domain.DTOs;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.Services.ApiDefinitions;
using CasosSospechososMI.Services.Interfaces;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CasosSospechososMI.Services.Account
{
    public class AccountService : IAccountService
    {
        IAuthenticatedService _apiService;
        HttpClient _httpClient;
        private const string UserKey = "user";
        private const string UserDataKey = "userData";
        private const string UserSupervisorDataKey = "userSupervisorData";
        private readonly string UserDefault = default(string);
        ICurrentConfiguration _currentConfituration;
        ISecureStorageService _secureStorageService;
        public AccountService(IHttpClientFactory httpClientFactory, ICurrentConfiguration currentConfiguration,
            ISecureStorageService secureStorageService, IAuthenticatedService apiService)
        {
            _apiService = apiService;
            _httpClient = httpClientFactory.CreateClient("AspersorClient");
            _currentConfituration = currentConfiguration;
            _secureStorageService = secureStorageService;
        }
        public DateTime LastSentSample { get; set; }
        public User ActualUser
        {
            get
            {
                var stringValue = AppSettings.GetValueOrDefault(UserKey, UserDefault);
                return string.IsNullOrEmpty(stringValue) ?
                    null :
                    JsonSerializer.Deserialize<User>(stringValue);
            }

            set
            {
                AppSettings.AddOrUpdateValue(UserKey, JsonSerializer.Serialize(value));
            }
        }
        public CasosSospechososMI.Domain.User.HomeDataModel HomeUserData
        {
            get
            {
                var stringValue = AppSettings.GetValueOrDefault(UserDataKey, UserDefault);
                return string.IsNullOrEmpty(stringValue) ?
                    null :
                    JsonSerializer.Deserialize<HomeDataModel>(stringValue);
            }

            set
            {
                AppSettings.AddOrUpdateValue(UserDataKey, JsonSerializer.Serialize(value));
            }
        }
        public CasosSospechososMI.Domain.User.HomeDataSupervisorModel HomeUserSupervisorData
        {
            get
            {
                var stringValue = AppSettings.GetValueOrDefault(UserSupervisorDataKey, UserDefault);
                return string.IsNullOrEmpty(stringValue) ?
                    null :
                    JsonSerializer.Deserialize<HomeDataSupervisorModel>(stringValue);
            }

            set
            {
                AppSettings.AddOrUpdateValue(UserSupervisorDataKey, JsonSerializer.Serialize(value));
            }
        }
        private ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        private async Task<OperationResult<ResponseGeneric>> RegistrationAsync(CancellationToken ct, string registerModel)
        {
            //var response = await _apiService.GetRestService<IAccountApiDefinition>().Registration(ct, registerModel);
            //return response;

            var responseFinal = new OperationResult<ResponseGeneric>();
            var current = Connectivity.NetworkAccess;
            var unavailableConnection = !(current == NetworkAccess.Internet);
            if (unavailableConnection)
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            var url = string.Format("{0}api/procev/casos/register", _httpClient.BaseAddress);

            var uri = new Uri(url);

            var data = new StringContent(registerModel, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(uri, data);

            if (response.IsSuccessStatusCode)
            {
                var regResponse = await response.Content.ReadAsStringAsync();
                responseFinal = JsonSerializer.Deserialize<OperationResult<ResponseGeneric>>(regResponse);
                return responseFinal;
            }
            responseFinal.Codigo = "1000";
            return responseFinal;
        }

        public async Task<OperationResult<ResponseGeneric>> RequestRegistrationAsync(CancellationToken ct, RegisterModel registerModel)
        {
            var param = string.IsNullOrEmpty(registerModel.Email) ? $@"nombre={registerModel.Name
                            }&apellido={registerModel.Surname
                            }&dni={registerModel.Dni
                            }&id_localidad={registerModel.CityId
                            }&domicilio={registerModel.Address
                            }&telefono={registerModel.Phone
                            }&password={registerModel.Password}" :
                            
                            $@"nombre={registerModel.Name
                            }&apellido={registerModel.Surname
                            }&dni={registerModel.Dni
                            }&email={registerModel.Email
                            }&id_localidad={registerModel.CityId
                            }&domicilio={registerModel.Address
                            }&telefono={registerModel.Phone
                            }&password={registerModel.Password}";

            return await RegistrationAsync(ct, param);
        }
        public async Task<LoginStateEnum> GetLoginStateAsync(CancellationToken ct)
        {
            //1 - Busco si es un usuario autenticado
            var haveUserAuthenticated = await HaveAutenticatedUser(ct);
            if (haveUserAuthenticated) return LoginStateEnum.UserAuthenticated;

            //2 - Si no tiene usuario autenticado, intento obtener token de app
            //comentamos para la primera entrega, solo validamos token de usuario
            //var haveAppAuthenticated = await HaveValidAppTokenAsync(ct);
            //if (haveAppAuthenticated) return LoginStateEnum.AppAuthenticated;

            else return LoginStateEnum.NotAuthenticated;
        }
        private async Task<bool> HaveAutenticatedUser(CancellationToken ct)
        {
            //si tiene datos de user en storage local
            if (ActualUser != null)
            {
                //verificamos si tiene token valido en storage
                return await HaveValidUserTokenAsync(ct);
            }
            return false;
        }
        //public async Task<bool> RequestFamilyTokenAsync(CancellationToken ct, string dni, string password)
        //{
        //    var param = $@"dni={dni
        //                    }&password={password
        //                    }";

        //    return await RequestAuthTokenAsync(ct, param);
        //}
        //public async Task<bool> RequestSupervisorTokenAsync(CancellationToken ct, string dni, string trapCode)
        //{
        //    var param = $@"dni={dni
        //                    }&password={trapCode
        //                    }";

        //    return await RequestAuthTokenAsync(ct, param);
        //} 
        public async Task<bool> RequestAuthTokenAsync(CancellationToken ct, string dni, string password)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                var unavailableConnection = !(current == NetworkAccess.Internet);
                if (unavailableConnection)
                {
                    return false;
                }

                //_httpClient.DefaultRequestHeaders
                //.Accept
                //.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                string url;
                string endpoint = $"api/procev/casos/login_agente?dni={dni}&password={password}";
                url = string.Format("{0}{1}", _httpClient.BaseAddress,endpoint);

                var uri = new Uri(url);

                //var data = new StringContent(param, Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await _httpClient.PostAsync(uri,null);

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await response.Content.ReadAsStringAsync();
                    var token = JsonSerializer.Deserialize<TokenDTO>(tokenResponse);

                    var handler = new JwtSecurityTokenHandler();
                    if (Convert.ToInt16(token.Codigo) == 0)
                    {

                        //var jsonToken = handler.ReadToken(token.AccessToken);
                        //var tokenS = jsonToken as JwtSecurityToken;
                        //token.ExpiresAt = DateTime.Now.AddSeconds(Convert.ToInt64(token.ExpiresIn));

                        //_secureStorageService.RemoveAuthToken();
                        _secureStorageService.SetAuthToken(token);

                        var user = new User();
                        user.DocumentNumber = Convert.ToInt32(dni);
                        user.Name = token.Nombre;
                        user.SurName = token.Apellido;
                        user.UserId = token.UserId;
                        user.Role = token.RoleId;
                        ActualUser = user;
                        return true;
                    }

                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                return false;
            }
        }
        public async Task<bool> HaveValidUserTokenAsync(CancellationToken ct)
        {
            var token = await _secureStorageService.GetAuthTokenAsync();
            if (token != null && ActualUser != null)
            {
                //if (DateTime.Now.AddHours(3) > token.ExpiresAt)
                //{
                //    var refreshTokenRequested = await RefreshTokenAsync(ct, ActualUser.UserId);
                //    if (!refreshTokenRequested)
                //    {
                //        //await LogOff();
                //    }
                //    return refreshTokenRequested;
                //}
                return true;
            }

            //await LogOff();
            return false;
        }
        public async Task LogOff(CancellationToken ct)
        {
            AppSettings.Remove(UserKey);
            _secureStorageService.RemoveAuthToken();
            ActualUser = null;
            //obtenemos token de app (para que sigan funcionando request de guest)
            //await RequestAppTokenAsync(ct);
        }
    }
}
