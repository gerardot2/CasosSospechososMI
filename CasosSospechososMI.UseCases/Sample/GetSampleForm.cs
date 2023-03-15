using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.Family.Interfaces;
using CasosSospechososMI.UseCases.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Sample
{
    public class GetSampleForm
    {
        IFamilyService _familyService;
        GetActualUser _getActual;

        public GetSampleForm(IFamilyService familyService, GetActualUser getActual)
        {
            _familyService = familyService;
            _getActual = getActual;
        }

        public async Task<OperationResult<List<FormModel>>> Invoke(CancellationToken ct)
        {
            var actual = _getActual.Invoke();
            var user = new HomeQuery()
            {
                TipoAplicacion = "5"
            };

            try
            {
                return await _familyService.GetFormItemsAsync(ct, user);
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
