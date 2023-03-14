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
    public class GetFamilyDataByCode
    {
        IFamilyService _familyService;

        public GetFamilyDataByCode(IFamilyService familyService)
        {
            _familyService = familyService;
        }

        public async Task<OperationResult<FamilyDataModel>> Invoke(CancellationToken ct, string code)
        {
            if (!string.IsNullOrEmpty(code))
            {

                try
                {
                    return await _familyService.GetFamilyDataByCodeAsync(ct,code);
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
