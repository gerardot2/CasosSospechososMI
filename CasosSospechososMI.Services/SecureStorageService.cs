using CasosSospechososMI.Domain.DTOs;
using CasosSospechososMI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CasosSospechososMI.Services
{
    public class SecureStorageService : ISecureStorageService
    {
        private const string AuthToken = "authToken";
        private const string AppDeviceGuid = "appDeviceGuid";

        #region AuthToken
        public async Task<TokenDTO> GetAuthTokenAsync()
        {
            try
            {
                //SecureStorage.RemoveAll();
                var authToken = await SecureStorage.GetAsync(AuthToken);
                return JsonSerializer.Deserialize<TokenDTO>(authToken);
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                return null;
            }

        }
        public async void SetAuthToken(TokenDTO token)
        {
            try
            {
                var authToken = JsonSerializer.Serialize(token);
                await SecureStorage.SetAsync(AuthToken, authToken);
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }

        }
        public bool RemoveAuthToken()
        {
            try
            {
                return SecureStorage.Remove(AuthToken);
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                return false;
            }

        }
        #endregion

        #region AppDeviceGuid
        public async Task<string> GetAppDeviceGuidAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(AppDeviceGuid);
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                return null;
            }

        }
        public async void SetAppDeviceGuid(string appDeviceGuid)
        {
            try
            {
                await SecureStorage.SetAsync(AppDeviceGuid, appDeviceGuid);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                //Crashes.TrackError(ex);
            }

        }
        #endregion

        public bool RemoveAllToken()
        {
            return RemoveAuthToken();
        }
    }
}
