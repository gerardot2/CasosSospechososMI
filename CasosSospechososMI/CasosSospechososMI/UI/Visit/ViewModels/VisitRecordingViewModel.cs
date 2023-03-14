using MosquitrampaMI.Domain.Samples;
using MosquitrampaMI.Services.Interfaces;
using MosquitrampaMI.UI.Base.ViewModels;
using MosquitrampaMI.UI.Records.Views;
using MosquitrampaMI.UseCases.Sample;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace MosquitrampaMI.UI.Visit.ViewModels
{
    public class VisitRecordingViewModel : BaseViewModel
    {
        #region Commands
        public ICommand RecordDetailCommand { get; }
        public ICommand SampleRecordCommand { get; }
        #endregion
        GetVisits _getMySamples;
        IRoutingService _routingService;
        public VisitRecordingViewModel(GetMySamples getMySamples, IRoutingService routingService)
        {
            RecordDetailCommand = new Command<SampleModel>((e) => OnRecordDetailClicked(e));
            SampleRecordCommand = new Command(() => OnNewVisitClicked());
            _getMySamples = getMySamples;
            _routingService = routingService;
            Muestras = new ObservableCollection<SampleModel>();
        }
        

        private async void OnNewVisitClicked()
        {
            if (IsBusy) return;

            IsBusy = true;
            await _routingService.NavigateTo($"///VisitRecording");
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
