using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CasosSospechososMI.UseCases.Common
{
    public class GetWhatsappNumber
    {
        ICommonService _commonService;
        IAccountService _accountService;

        public GetWhatsappNumber(ICommonService commonService, IAccountService accountService)
        {
            _commonService = commonService;
            _accountService = accountService;
        }

        public async Task<string> Invoke(CancellationToken ct)
        {
            try
            {
                var number = await _commonService.GetWhatsappNumberAsync(ct);
                return number.Numero;
            }
            catch (TaskCanceledException ex)
            {
                //si cancelo la peticion, no devuelvo nada
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
