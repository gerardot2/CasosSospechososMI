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
    public class GetLastVersion
    {
        ICommonService _commonService;
        IAccountService _accountService;

        public GetLastVersion(ICommonService commonService, IAccountService accountService)
        {
            _commonService = commonService;
            _accountService = accountService;
        }

        public async Task<VersionModel> Invoke(CancellationToken ct)
        {
            try
            {
                return await _commonService.GetLastVersionAsync(ct);
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
