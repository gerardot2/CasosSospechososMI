using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.Services.Common.Interfaces;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UI.Registration.Views;
using CasosSospechososMI.UseCases.Common;
using CasosSospechososMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CasosSospechososMI.UI.Login.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand OnRegistrationTappedCommand { get; }
        public ICommand OnLoginTappedCommand { get; }
        IAccountService _accountService;
        GetWhatsappNumber _getWhatsapp;
        public LoginViewModel(IAccountService accountService, GetWhatsappNumber getWhatsapp, IRoutingService routingService) : base(routingService)
        {

            _accountService = accountService;
            _getWhatsapp = getWhatsapp;
            OnRegistrationTappedCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushModalAsync(new RegistrationPage(IsSupervisor)));
            OnLoginTappedCommand = new Command(OnLoginTapped);
        }

        private async void OnLoginTapped(object obj)
        {
            IsBusy = true;
            //----------- Remover cuando implem supervisor---------------
            //if (IsSupervisor)
            //{
            //    IsBusy = false;
            //    await OpenResultWindow("Acceso", "Ésta función se habilitará proximamente.");
            //    return;
            //}
            //------------------------------------------------------------
            if ((UserName != null && UserName.Length == 8 && Password != null && Password.Length > 5 && Password.Length < 11) || (UserName != null && UserName.Length == 8 && TrapCode != null))
            {
                var result = await _accountService.RequestAuthTokenAsync(CancellationTokenSource.Token, UserName, Password, TrapCode);

            if (result)
            {
                if (!_accountService.ActualUser.Supervisor)
                {
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    Application.Current.MainPage = new SupervisorShell();
                }
            }
            else
            {
                IsBusy = false;
                await OpenResultWindow("Acceso", "Ocurrió un error, por favor intente de nuevo.");
            }
        }
            IsBusy = false;
        }

        #region Properties
        string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
            }
        }
        string _password;

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
            }
        }
        string _trapCode;
        public string TrapCode
        {
            get => _trapCode;
            set
            {
                SetProperty(ref _trapCode, value);
            }
        }
        #endregion
        #region Actions

        public async void OnAppearing()
        {
            CancellationTokenSource = new System.Threading.CancellationTokenSource();
            var numero = await _getWhatsapp.Invoke(CancellationTokenSource.Token);
            App.WhatsappNumber = !string.IsNullOrEmpty(numero) ? numero : string.Empty; 
        }
        #endregion
    }
}
