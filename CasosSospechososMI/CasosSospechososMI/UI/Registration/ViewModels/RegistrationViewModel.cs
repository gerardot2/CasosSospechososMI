using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UI.Common.Views;
using CasosSospechososMI.UI.Login.Views;
using CasosSospechososMI.UseCases.Account;
using CasosSospechososMI.UseCases.Common;
using CasosSospechososMI.ViewModels;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CasosSospechososMI.UI.Registration.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        public Command RegistrationCommand { get; }
        PostRegistration _registrationService;
        GetCities _getCities;
        GetWhatsappNumber _getWhatsapp;
        public RegistrationViewModel(PostRegistration registrationService,
            GetCities getCities,
            GetWhatsappNumber getWhataspp,
            IRoutingService routingService) : base(routingService)
        {
            RegistrationCommand = new Command(OnRegistrationClicked);
            _registrationService = registrationService;
            _getCities = getCities;
            _getWhatsapp = getWhataspp;
            RegisterModel = new RegisterModel();
            SelectedCity = new City();
            Cities = new ObservableCollection<City>();
        }
        RegisterModel _registerModel;
        public RegisterModel RegisterModel
        {
            get => _registerModel;
            set
            {
                SetProperty(ref _registerModel, value);
            }
        }
        ObservableCollection<City> _cities;
        public ObservableCollection<City> Cities
        {
            get => _cities;
            set
            {
                SetProperty(ref _cities, value);
            }
        }
        City _selectedCity;
        public City SelectedCity
        {
            get => _selectedCity;
            set
            {
                SetProperty(ref _selectedCity, value);
            }
        }
        bool _passMatch;
        public bool PassMatch
        {
            get => _passMatch;
            set
            {
                SetProperty(ref _passMatch, value);
            }
        }
        public async void OnRegistrationClicked(object obj)
        {
            if (IsBusy) return;
            IsBusy = true;
            RegisterModel.CityId = (SelectedCity != null) ? (int)SelectedCity.Id : 0;
            RegisterModel.RoleId = IsSupervisor ? 3: 4 ;
            if (RegisterModel.IsFulFilled || (RegisterModel.IsSupervisorFulFilled && PassMatch))
            {
                var result = await _registrationService.Invoke(CancellationTokenSource.Token, RegisterModel, IsSupervisor);
                if (result != null && int.Parse(result.Codigo) == 0 && !IsSupervisor)
                {
                    IsBusy = false;

                    await OpenResultWindow("Registro", "¡Se ha registrado con éxito!\n\nInicie sesión con sus datos.", Pr_GoToLogin);

                }
                else if (result != null && int.Parse(result.Codigo) == 0 && IsSupervisor)
                {
                    IsBusy = false;

                    await OpenResultWindow("Registro", "¡Se ha registrado con éxito!\n\nPodrá ingresar luego de que un administrador lo habilite.", Pr_GoToLogin);

                }
                else if (result != null && result.Mensaje != null && int.Parse(result.Codigo) > 0)
                {
                    IsBusy = false;
                    //await OpenResultWindow("Registro", $"Error\n\n{result.Mensaje}.");
                    await OpenResultWindow("Registro", $"Error\n\n{result.Mensaje}.");
                }
                else if (result != null && result.Mensaje != null && int.Parse(result.Codigo) == 0)
                {
                    IsBusy = false;
                    //await OpenResultWindow("Registro", $"Error\n\n{result.Mensaje}.");
                    await OpenResultWindow("Registro", $"{result.Mensaje}");
                }
                else
                {
                    IsBusy = false;
                    await OpenResultWindow("Registro", $"Error\n\nPor favor intente de nuevo.");
                }

            }
            
            else
            {
                IsBusy = false;

                await OpenResultWindow("Registro", "Error\n\nPor favor complete y verifique los datos.");
            }
            IsBusy = false;
        }

        private void Pr_GoToLogin(object sender, object e)
        {
            App.Current.MainPage = new LoginPage(IsSupervisor);
        }

        internal void OnAppearing()
        {
            CancellationTokenSource = new CancellationTokenSource();

            SeachCities().ConfigureAwait(false);
        }

        public async Task SeachCities()
        {
            if (IsBusy) return;
            IsBusy = true;
            var result = await _getCities.Invoke(CancellationTokenSource.Token);
            if (result != null
                && result.Data != null)
            {
                Cities = new ObservableCollection<City>(result.Data);
            }
            else
            {
                Cities = new ObservableCollection<City>();
            }
            if (string.IsNullOrEmpty(App.WhatsappNumber))
            {
                var numero = await _getWhatsapp.Invoke(CancellationTokenSource.Token);
                App.WhatsappNumber = !string.IsNullOrEmpty(numero) ? numero : string.Empty;
            }
            IsBusy = false;
        }
    }
}
