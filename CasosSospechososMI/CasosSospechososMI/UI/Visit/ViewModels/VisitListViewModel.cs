using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UseCases.Family;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CasosSospechososMI.UI.Visit.ViewModels
{
    public class VisitListViewModel : BaseViewModel
    {
        public ICommand OnRegisterVisitCommand { get; }
        public ICommand OnBackPressCommand { get; }
        
        IRoutingService _routingService;
        GetSamplesByCode _getVisitsById;
        public VisitListViewModel(IRoutingService routingService, GetSamplesByCode getVisitsById) : base(routingService)
        {
            _routingService = routingService;
            _getVisitsById = getVisitsById;
            OnRegisterVisitCommand = new Command(()=> OnRegisterVisitClicked());
            Visits = new ObservableCollection<SampleModel>();
            SelectedFamily = new FamilyDataModel();
            //OnBackPressCommand = new Command(Pr_GoBackHome);
        }

        private async void OnRegisterVisitClicked()
        {
            if (IsBusy) return;
            IsBusy = true;
            await Shell.Current.GoToAsync($"SampleRecording?code={SelectedFamily.CodigoOvitrampa}");
            IsBusy = false;
        }
        string _code;
        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);

            }
        }
        ObservableCollection<SampleModel> _visits;
        public ObservableCollection<SampleModel> Visits
        {
            get => _visits;
            set
            {
                SetProperty(ref _visits, value);

            }
        }
        FamilyDataModel _selectedFamily;
        public FamilyDataModel SelectedFamily
        {
            get => _selectedFamily;
            set
            {
                SetProperty(ref _selectedFamily, value);

            }
        }
        bool _hasVisits;
        public bool HasVisits
        {
            get => _hasVisits;
            set
            {
                SetProperty(ref _hasVisits, value);

            }
        }
        internal async void OnAppearing()
        {
            CancellationTokenSource = new CancellationTokenSource();
            SearchVisitsAsync().ConfigureAwait(false);
        }

        private async Task SearchVisitsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            var result = await _getVisitsById.Invoke(CancellationTokenSource.Token, SelectedFamily.CodigoOvitrampa);
            if (result != null)
            {
                if (result != null && result.Data != null)
                {
                    HasVisits = true;
                    Visits = new ObservableCollection<SampleModel>(result.Data);
                }
                else
                {
                    Visits = new ObservableCollection<SampleModel>();
                    
                }
            }
            else if (int.Parse(result.Codigo) == 0 && !string.IsNullOrEmpty(result.Mensaje))
            {
                IsBusy = false;
                await OpenResultWindow("Error de Datos", $"Hubo un error obteniendo datos.\n{result.Mensaje}");
            }
            else
            {
                IsBusy = false;
                await OpenResultWindow("Error de Datos", "Hubo un error obteniendo datos.\nSi el error persiste, cierre sesión y vuelva a ingresar.", Pr_GoBackHome);
            }
            IsBusy= false;
        }
        private async void Pr_GoBackHome(object sender, object e)
        {
            await _routingService.GoBackModal();
        }
    }
}
