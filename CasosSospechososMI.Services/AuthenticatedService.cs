using CasosSospechososMI.Domain.DTOs;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CasosSospechososMI.Services
{
    public class AuthenticatedService : IAuthenticatedService
    {
        HttpClient _client;
        public AuthenticatedService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("AspersorClient");

            var token = GetAuthToken();
            if (token != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            }
        }

        public T GetRestService<T>()
        {
            return RestService.For<T>(_client);
        }

        private TokenDTO GetAuthToken()
        {
            var task = Task.Run<TokenDTO>(async () => await GetAuthTokenAsync());
            return task.Result;
        }

        public async Task UpdateAuthToken()
        {
            var token = await GetAuthTokenAsync();
            if (token != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            }
        }

        private async Task<TokenDTO> GetAuthTokenAsync()
        {
            try
            {
                var authToken = await SecureStorage.GetAsync("authToken");
                if (authToken != null)
                {
                    return JsonSerializer.Deserialize<TokenDTO>(authToken);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                return null;
            }

        }
    }
}
