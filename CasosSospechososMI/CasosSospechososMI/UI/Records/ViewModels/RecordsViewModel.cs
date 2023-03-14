using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UseCases.Sample;
using CasosSospechososMI.Domain.Samples;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Records.Views;

namespace CasosSospechososMI.UI.Records.ViewModels
{
    public class RecordsViewModel : BaseViewModel
    {
        #region Commands
        public ICommand RecordDetailCommand { get; }
        public ICommand SampleRecordCommand { get; }
        #endregion
        GetMySamples _getMySamples;
        IRoutingService _routingService;
        public RecordsViewModel(GetMySamples getMySamples, IRoutingService routingService) : base(routingService)
        {
            RecordDetailCommand = new Command<SampleModel>((e) => OnRecordDetailClicked(e));
            SampleRecordCommand = new Command(() => OnNewSampleClicked());
            _getMySamples = getMySamples;
            _routingService = routingService;
            Muestras = new ObservableCollection<SampleModel>();
        }

        private async void OnNewSampleClicked()
        {
            if (IsBusy) return;
            IsBusy = true;
            await _routingService.NavigateTo($"///SampleRecording");
            IsBusy = false;
        }

        private async void OnRecordDetailClicked(SampleModel e)
        {
            if (IsBusy) return;

            IsBusy = true;
            await _routingService.PushModal(new RecordDetailPage(e));
            IsBusy = false;
        }
        #region Properties
        ObservableCollection<SampleModel> _muestras;
        public ObservableCollection<SampleModel> Muestras
        {
            get => _muestras;
            set
            {
                SetProperty(ref _muestras, value);
            }
        }
        bool _hasSamples = false;
        public bool HasSamples
        {
            get => _hasSamples;
            set
            {
                SetProperty(ref _hasSamples, value);
            }
        }
        #endregion 
        internal async void OnAppearing()
        {
            CancellationTokenSource = new CancellationTokenSource();
            if (!IsConnected)
            {
                await OpenResultWindow("Sin Conexión", "Hubo un error obteniendo datos.\nSi el error persiste, cierre sesión y vuelva a ingresar.", Pr_VolverInicio);
            }
            await GetRecorderedSamples();
        }

        private async Task GetRecorderedSamples()
        {
            if (IsBusy) return;
            IsBusy = true;
            var result = await _getMySamples.Invoke(CancellationTokenSource.Token);
            if (result != null && int.Parse(result.Codigo) == 0 && result.Data.Any())
            {
                HasSamples = true;
                Muestras = new ObservableCollection<SampleModel>(result.Data);
            }
            else
            {
                Muestras = new ObservableCollection<SampleModel>();
                //await OpenResultWindow("Atención", "Todavía no ha registrado muestras.");
            }
            IsBusy = false;
        }
    }
}
