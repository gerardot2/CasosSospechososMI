using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Common
{
    public class GetCities
    {
        ICommonService _commonService;

        public GetCities(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public async Task<OperationResult<List<City>>> Invoke(CancellationToken ct)
        {
            try
            {
                var cities = await _commonService.GetCitiesAsync(ct);
                return cities; 
            }
            catch (TaskCanceledException ex)
            {
                //si cancelo la peticion, no devuelvo nada
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
