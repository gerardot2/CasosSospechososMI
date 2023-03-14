using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Services.Family.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Family
{
    public class PostSaveFamilyDataChanges
    {
        IFamilyService _familyService;
        public PostSaveFamilyDataChanges(IFamilyService familyService)
        {
            _familyService = familyService;
        }
        public async Task<ResponseGeneric> Invoke(CancellationToken cancellationToken, RegisterModel user)
        {
            try
            {
                return await _familyService.PostSaveDataUpdateAsync(cancellationToken, user);
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
