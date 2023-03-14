using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.Services.Family.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Family
{
    public class GetSamplesByCode
    {
        IFamilyService _familyService;
        public GetSamplesByCode(IFamilyService familyService)
        {
            _familyService = familyService;
        }
        public async Task<OperationResult<List<SampleModel>>> Invoke(CancellationToken ct, string id)
        {
            try
            {
                var result = await _familyService.GetVisitsByIdAsync(ct, id);
                if (result != null && result.Data != null && result.Data.Any())
                {
                    result.Data = result.Data.OrderByDescending(x => x.Id).ToList();
                }else if (result != null && int.Parse(result.Codigo) == 0)
                {
                    return result;
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
