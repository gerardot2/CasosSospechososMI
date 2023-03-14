using CasosSospechososMI.Models;
using CasosSospechososMI.Services;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Common.Views;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;

namespace CasosSospechososMI.UI.Base.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        IRoutingService _routingService;
        public Command WhatsappCommand { get; }
        public Command OnCloseModalCommand { get; }
        public BaseViewModel(IRoutingService routingService)
        {

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var current = Connectivity.NetworkAccess;
            IsConnected = current == NetworkAccess.Internet;
            WhatsappCommand = new Command<string>((e) => OnNewWhatsappCommand(e));
            OnCloseModalCommand = new Command(() => OnCloseModalClicked());
            DeviceDisplayHeight = App.DeviceDisplayHeight;
            DeviceDisplayWidth = App.DeviceDisplayWidth;
            _routingService = routingService;
        }
        private async void OnCloseModalClicked()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void OnNewWhatsappCommand(string e = null)
        {
            if (!string.IsNullOrEmpty(App.WhatsappNumber))
            {
                try
                {
                    //var number = Regex.Replace(App.WhatsappNumber.ToString(), "[^0-9]", "");
                    //if (number[0] == '0')
                    //{
                    //    number = number.Substring(1);
                    //}
                    var message = !string.IsNullOrEmpty(e) ? e : string.Empty;
                    var number = App.WhatsappNumber;
                    if (IsBusy) return;
                    IsBusy = true;

                    Chat.Open(number,message);
                    //bool supportsUri = await Launcher.CanOpenAsync($"https://api.whatsapp.com/send?phone={number}");

                    //if (supportsUri)
                    //    await Launcher.OpenAsync($"https://api.whatsapp.com/send?phone={number}");
                    //var uri = new Uri($"https://wa.me/54{number}&text=");
                    //await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    return;
                }
                finally
                {
                    IsBusy = false;
                }

            }
        }
        #region Properties
        public CancellationTokenSource CancellationTokenSource { get; set; }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value);
                MessagingCenter.Send(new GenericMessage(), "HideNavBar", isBusy);
            }
        }
        bool isNotConnected = false;
        public bool IsNotConnected
        {
            get { return isNotConnected; }
            set { SetProperty(ref isNotConnected, value); }
        }
        bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                IsNotConnected = !value;
                SetProperty(ref isConnected, value);
            }
        }
        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        double deviceDisplayHeight;
        public double DeviceDisplayHeight
        {
            get { return deviceDisplayHeight; }
            set { SetProperty(ref deviceDisplayHeight, value); }
        }
        double deviceDisplayWidth;
        public double DeviceDisplayWidth
        {
            get { return deviceDisplayWidth; }
            set { SetProperty(ref deviceDisplayWidth, value); }
        }
        bool _isSupervisor;
        public bool IsSupervisor
        {
            get => _isSupervisor;
            set
            {
                SetProperty(ref _isSupervisor, value);
            }
        }
        #endregion

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //var changed = PropertyChanged;
            //if (changed == null)
            //    return;

            //changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //var current = e.NetworkAccess;
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
            MessagingCenter.Send(new GenericMessage(), "Connection", IsConnected);
            //bool shown = false;
            //if (e.NetworkAccess != NetworkAccess.Internet && !shown)
            //{
            //    await OpenResultWindow("Conexión", "Conectese a internet para enviar los datos almacenados en local antes de agregar un nuevo registro.");
            //    shown = true;
            //    return;
            //}
        }
        public async void Pr_VolverInicio(object sender, object e)
        {
            await _routingService.NavigateTo("///AboutPage");
        }
        public async void Pr_KillApp(object sender, object e)
        {
            MessagingCenter.Send(new GenericMessage(), "OnlyShutdown");
        }
        public async Task OpenResultWindow(string title, string message, EventHandler<object> eventHandler = null)
        {
            var pr = new ResultPopup(title, message);
            var scaleAnimation = new ScaleAnimation
            {
                PositionIn = MoveAnimationOptions.Right,
                PositionOut = MoveAnimationOptions.Left
            };

            pr.Animation = scaleAnimation;
            pr.CloseWhenBackgroundIsClicked = false;
            pr.OnConfirmEvent += eventHandler;
            await PopupNavigation.Instance.PushAsync(pr);
        }
        public async Task OpenCustomPopup(string title,
                                        string body,
                                        string confirm,
                                        EventHandler<object> OnConfirmEvent = null,
                                        EventHandler<object> OnCloseEvent = null,
                                        bool alert = false
                                        )
        {

            var pr = new ConfirmPopup(title, body, confirm, OnConfirmEvent,OnCloseEvent,alert);
            var scaleAnimation = new ScaleAnimation
            {
                PositionIn = MoveAnimationOptions.Right,
                PositionOut = MoveAnimationOptions.Left
            };

            pr.Animation = scaleAnimation;
            pr.CloseWhenBackgroundIsClicked = false;
            pr.OnConfirmEvent += OnConfirmEvent;
            pr.OnCloseEvent += OnCloseEvent;


            await PopupNavigation.Instance.PushAsync(pr);
        }
        public bool IsConnection() 
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

    }
}
