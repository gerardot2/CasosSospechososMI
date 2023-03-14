using CasosSospechososMI.Services.Family.Interfaces;
using CasosSospechososMI.UseCases.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.Common;

namespace CasosSospechososMI.UseCases.Family
{
    public class GetFamilyData
    {
        IFamilyService _familyService;
        GetActualUser _getActual;

        public GetFamilyData(IFamilyService familyService, GetActualUser getActual)
        {
            _familyService = familyService;
            _getActual = getActual;
        }

        public async Task<OperationResult<FamilyDataModel>> Invoke(CancellationToken ct)
        {
            var actual = _getActual.Invoke();
            if (actual != null)
            {

                try
                {
                    return await _familyService.GetFamilyDataAsync(ct,actual.UserId.Value);
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
            return null;
        }
    }
}
