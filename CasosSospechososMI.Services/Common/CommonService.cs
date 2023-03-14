using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Services.ApiDefinitions;
using CasosSospechososMI.Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services.Common
{
    public class CommonService : ICommonService
    {
        IAuthenticatedService _apiService;
        public CommonService(IAuthenticatedService apiService)
        {
            _apiService = apiService;
        }
        public async Task<OperationResult<List<City>>> GetCitiesAsync(CancellationToken ct)
        {
            return await _apiService.GetRestService<ICommonApiDefinition>().GetCities(ct);
        }
        public async Task<OperationResult<ResponseGeneric>> GetWhatsappNumberAsync(CancellationToken ct)
        {
            return await _apiService.GetRestService<ICommonApiDefinition>().GetWhatsappNumber(ct);
        }
        public async Task<VersionModel> GetLastVersionAsync(CancellationToken ct)
        {
            return await _apiService.GetRestService<ICommonApiDefinition>().GetLastVersion(ct);
        }
    }
}
