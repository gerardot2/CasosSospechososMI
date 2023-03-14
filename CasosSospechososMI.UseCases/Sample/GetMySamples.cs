using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.Services.Family.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Sample
{
    public class GetMySamples
    {
        IFamilyService _familyService;

        public GetMySamples(IFamilyService familyService)
        {
            _familyService = familyService;
        }

        public async Task<OperationResult<List<SampleModel>>> Invoke(CancellationToken ct)
        {

            try
            {
                var result = await _familyService.GetMySamplesAsync(ct);
                if (result != null && result.Data.Any())
                {
                    result.Data = result.Data.OrderByDescending(x => x.Id).ToList();
                }
                return result;
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
