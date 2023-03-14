using CasosSospechososMI.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Account
{
    public class LogOut
    {
        IAccountService _accountService;

        public LogOut(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Invoke(CancellationToken ct)
        {
            try
            {
                await _accountService.LogOff(ct);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
