using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.Services.Family.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ZXing;

namespace CasosSospechososMI.UseCases.Account
{
    public class GetHomeData
    {
        IFamilyService _familyService;
        IAccountService _usuarioService;
        GetActualUser _getActual;

        public GetHomeData(IFamilyService familyService,
            IAccountService usuarioService,
        GetActualUser getActual)
        {
            _familyService = familyService;
            _usuarioService = usuarioService;
            _getActual = getActual;
        }

        public async Task<HomeDataModel> Invoke(CancellationToken ct)
        {
            var actual = _getActual.Invoke();

            try
            {
                var resp = new HomeDataModel();
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    resp = await _familyService.GetSupervisorHomeDataAsync(ct);

                    if (resp !=null && int.Parse(resp.Codigo) == 0)
                    {
                        _usuarioService.HomeUserData = resp;
                    }


                }
                else
                {
                    return _usuarioService.HomeUserData;
                }
                return resp;
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
