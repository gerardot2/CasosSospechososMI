using Akavache;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UI.Common.Views;
using CasosSospechososMI.UI.Login.Views;
using CasosSospechososMI.UseCases.Account;
using CasosSospechososMI.UseCases.Common;
using CasosSospechososMI.UseCases.Sample;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Reactive.Linq;
using CasosSospechososMI.UseCases.Supervisor;
using System.Linq;
using Refit;
using System.IO;

namespace CasosSospechososMI.UI.Home.ViewModels
{
    
    public class AboutViewModel : BaseViewModel
    {
        IRoutingService _routingService;
        GetHomeData _getHomeData;
        GetHomeUserData _getHomeUserData;
        UpdateHomeUserData _updateHomeUserData;
        GetWhatsappNumber _getWhatsapp;
        GetActualUser _getActual;
        GetHomeUserSupervisorData _getSuperData;
        GetSampleForm _getSampleForm;
        PostSampleRecord _postSampleRecord;
        PostVisitRecord _postVisitRecord;
        UpdateActualUser _updateActualUser;
        GetLastVersion _getLastVersion;
        public ICommand LoginButtonCommand { get; }
        public ICommand MenuModalCommand { get; }
        public ICommand OpenWebCommand { get; }
        public ICommand OnPlayVideoClicked { get; }

        VersionModel version;
        public AboutViewModel(IRoutingService routingService, 
            GetHomeData getHomeData, 
            GetHomeUserData getHomeUserData, 
            UpdateHomeUserData updateHomeUserData,
            GetWhatsappNumber getWhatsapp,
            GetActualUser getActualUser,
            GetSampleForm getSampleForm,
            PostSampleRecord postSampleRecord,
            PostVisitRecord postVisitRecord,
            GetHomeUserSupervisorData getSupervisorData,
            UpdateActualUser updateActualUser,
            GetLastVersion getLastVersion) : base(routingService)
        {
            _routingService = routingService;
            _getHomeData = getHomeData;
            _getHomeUserData = getHomeUserData;
            _updateHomeUserData = updateHomeUserData;
            _getWhatsapp = getWhatsapp;
            _getActual = getActualUser;
            _getSampleForm = getSampleForm;
            _postSampleRecord = postSampleRecord;
            _postVisitRecord = postVisitRecord;
            _updateActualUser = updateActualUser;
            _getLastVersion = getLastVersion;
            _getSuperData = getSupervisorData;

            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            OnPlayVideoClicked = new Command( async () => await OpenPlayerView());
            //LoginButtonCommand = new Command(async () => await _routingService.NavigateTo($"///main/LoginPage"));
            LoginButtonCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new LoginPage()));
            MenuModalCommand = new Command(() => Shell.Current.FlyoutIsPresented = true);
        }

        private async Task OpenPlayerView(EventHandler<object> eventHandler = null)
        {
            if (IsBusy) return;

            IsBusy = true;
            await _routingService.PushModal(new PlayerPage());
            IsBusy = false;
        }

        HomeDataModel _homeData;
        public HomeDataModel HomeData
        {
            get => _homeData;
            set
            {
                SetProperty(ref _homeData, value);
            }
        }
        HomeDataSupervisorModel _homeSupervisorData;
        public HomeDataSupervisorModel HomeSupervisorData
        {
            get => _homeSupervisorData;
            set
            {
                SetProperty(ref _homeSupervisorData, value);
            }
        }
       
        internal async void OnAppearing()
        {
            CancellationTokenSource = new CancellationTokenSource();
            if (!IsConnected) { await OpenResultWindow("Conexión", "Conectese a internet para enviar los datos almacenados en local antes de agregar un nuevo registro."); }
            else
            {
                version = await _getLastVersion.Invoke(CancellationTokenSource.Token);
                if (version != null && VersionTracking.CurrentVersion != version.Version
                    ) { 
                
                    await OpenCustomPopup("Actualización","Hay una nueva versión, es necesario actualizar.","Descargar",IrALink, Pr_KillApp, true);
                }
            }
            await GetHomeData();
        }

        private async void IrALink(object sender, object e)
        {

            if (!string.IsNullOrEmpty(version.Link))
            {
                await Browser.OpenAsync(version.Link, BrowserLaunchMode.SystemPreferred);
            }
        }

        private async Task GetHomeData()
        {
            if (IsBusy) return;
            IsBusy = true;
            var actualUser =  _getActual.Invoke();
            
            var result = await _getHomeData.Invoke(CancellationTokenSource.Token);
            if (result != null)
            {
                    HomeSupervisorData = (HomeDataSupervisorModel)result;
                
                if (!string.IsNullOrEmpty(result.AvisoCabecera) && !string.IsNullOrEmpty(result.AvisoTexto))
                {
                    await OpenResultWindow(result.AvisoCabecera,result.AvisoTexto);
                }
            }
            else
            {
                
                var storedSupervisorData = _getSuperData.Invoke();
                if (storedSupervisorData != null) HomeSupervisorData = storedSupervisorData;
                
            }

            await CheckCachedData();
            if (IsConnected)
            {

                var numero = await _getWhatsapp.Invoke(CancellationTokenSource.Token);
                App.WhatsappNumber = !string.IsNullOrEmpty(numero) ? numero : string.Empty;
            }

            IsBusy = false;
            
            if (HomeSupervisorData == null) await OpenResultWindow("Error de Datos", "Hubo un error obteniendo datos.\nSi el error persiste, cierre sesión y vuelva a ingresar.");
        }

        private async Task CheckCachedData()
        {
            OperationResult<List<FormModel>> formModel;
            var resp = new OperationResult<List<FormModel>>();
            Akavache.Registrations.Start("CasosSospechososMI");
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains("formModel"))
            {
                resp = await BlobCache.LocalMachine.GetObject<OperationResult<List<FormModel>>>("formModel");
                var update = _getActual.Invoke();
                if (update != null) { update.HasCachedForm = true; _updateActualUser.Invoke(update); };
            }
            else
            {
                var update = _getActual.Invoke();
                if (update != null) { update.HasCachedForm = false; _updateActualUser.Invoke(update); };
                //App.ActualUser.HasCachedForm = false;
            }

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    //Actualizar form de muestra
                    await BlobCache.LocalMachine.InvalidateObject<OperationResult<List<FormModel>>>("formModel");
                    formModel = await _getSampleForm.Invoke(CancellationTokenSource.Token);
                    if (formModel != null)
                    {
                        await BlobCache.LocalMachine.InsertObject("formModel", formModel);
                        var update = _getActual.Invoke();
                        if (update != null) { update.HasCachedForm = true; _updateActualUser.Invoke(update); };
                    }

                    //Si existe enviar registro de muestra
                    if (keys.Contains("sampleModel"))
                    {
                        var result = new OperationResult<ResponseGeneric>();
                        
                        var sampleSuperv = await BlobCache.LocalMachine.GetObject<(FormRecordModel FormRecord, byte[] Image, string Code)>("sampleModel");
                        StreamPart image = ConvertByteToStreamPart(sampleSuperv.Image);
                        result = await _postVisitRecord.Invoke(CancellationTokenSource.Token,sampleSuperv.FormRecord,image,sampleSuperv.Code);
                           
                        
                        if (result != null && int.Parse(result.Codigo) == 0)
                        {
                           
                            await BlobCache.LocalMachine.InvalidateObject<(FormRecordModel, byte[], string)>("sampleModel");
                            var update = _getActual.Invoke();
                            if (update != null) { update.HasCachedRecord = false; _updateActualUser.Invoke(update); };
                            
                        }
                    }

                }
                catch (Exception ex)
                {

                    Console.Write(ex);
                }

                return;
            }
            try
            {

                if (resp != null && resp.Data != null)
                {
                    var update = _getActual.Invoke();
                    if (update != null) { update.HasCachedForm = true; _updateActualUser.Invoke(update); };
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private StreamPart ConvertByteToStreamPart(byte[] imagen)
        {
            Stream stream = new MemoryStream(imagen);
            string name = IsSupervisor ? "visita.jpg" : "muestra.jpg";
            return new StreamPart(stream, name
                    , contentType: MediaType.ImageDefault
                    );
        }
    }
}