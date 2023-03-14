using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Account
{
    public class PostRegistration
    {
        IAccountService _accountService;

        public PostRegistration(IAccountService accountService)
        {
            _accountService = accountService;

        }

        public async Task<OperationResult<ResponseGeneric>> Invoke(CancellationToken ct, RegisterModel registerModel, bool supervisor)
        {
            var response = new OperationResult<ResponseGeneric>();
            try
            {
                return await _accountService.RequestRegistrationAsync(ct, registerModel, supervisor);
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
