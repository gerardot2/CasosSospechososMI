using CasosSospechososMI.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services.Interfaces
{
    public interface ISecureStorageService
    {
        Task<TokenDTO> GetAuthTokenAsync();
        void SetAuthToken(TokenDTO token);
        bool RemoveAuthToken();

        Task<string> GetAppDeviceGuidAsync();
        void SetAppDeviceGuid(string appDeviceGuid);

        bool RemoveAllToken();
    }
}
