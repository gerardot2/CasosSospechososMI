using Acr.UserDialogs;
using Android.Content.Res;
using CasosSospechososMI.Models;
using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UI.Login.Views;
using CasosSospechososMI.UseCases.Account;
using CasosSospechososMI.ViewModels;
using CasosSospechososMI.Views;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;
using Xamarin.Forms.PlatformConfiguration;

namespace CasosSospechososMI
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public Dictionary<string, Type> Routes { get { return routes; } }
        private bool _connected;
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            MessagingCenter.Subscribe<GenericMessage, bool>(this, "Connection", (obj, sender) =>
            {
                _connected = sender;
                homeTab.IsEnabled = _connected;
                dataTab.IsEnabled = _connected;
                recordsTab.IsEnabled = _connected;
                whatsappIcon.IsEnabled = _connected;
                protocolTab.IsEnabled = _connected;
            });
            MessagingCenter.Subscribe<GenericMessage,bool>(this, "HideNavBar", (obj,sender) =>
            {
                Shell.SetTabBarIsVisible(this, !sender);
            });
            versionLabel.Text = $"Ver. {VersionTracking.CurrentVersion}";
        }
        void RegisterRoutes()
        {
            routes.Add("ItemDetailPage", typeof(ItemDetailPage));
            routes.Add("NewItemPage", typeof(NewItemPage));
            routes.Add("LoginPage", typeof(CasosSospechososMI.UI.Login.Views.LoginPage));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }

        }
        private void OnMenuItemClicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () => {
                var config = new ConfirmConfig()
                {
                    Title = "Atención ",
                    Message = $"¿Está seguro que desea salir?",
                    OkText = "Si",
                    CancelText = "No",
                    AndroidStyleId = 2131689474,
                    
                };
                var confirm = await UserDialogs.Instance.ConfirmAsync(config);
                if (confirm)
                {
                    
                    MessagingCenter.Send(new GenericMessage(), "Shutdown");
                }
            });
        }
        private void OnWhatsappClicked(object sender, EventArgs e)
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
                    var number = App.WhatsappNumber;
                    if (IsBusy) return;
                    IsBusy = true;
                    Chat.Open(number);

                    //bool supportsUri = await Launcher.CanOpenAsync($"whatsapp://send?phone={number}");

                    //if (supportsUri)
                    //    await Launcher.OpenAsync($"whatsapp://send?phone={number}&text=Hola, soy el usuario Nro {App.ActualUser.UserId}, {App.ActualUser.Name} {App.ActualUser.SurName}");
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
        private async void OnProtocolClicked(object sender, EventArgs e)
        {
            
            //var filePath = "android_asset/protocol.pdf";

            //if (filePath != null)
            //{
            //    await Launcher.OpenAsync(new OpenFileRequest
            //    {
            //        File = new ReadOnlyFile(filePath)
            //    });
            //}
        }
        protected override bool OnBackButtonPressed()
        {
            if (Shell.Current.Navigation.NavigationStack.Count == 1)
            {
                var current = Shell.Current.CurrentItem.CurrentItem.Route;
                if (current.Contains("AboutPage"))
                {
                    return base.OnBackButtonPressed();
                }
                else
                {
                    Shell.Current.GoToAsync($"///main/AboutPage");
                }
                return true;
            }
            else
            {

                return base.OnBackButtonPressed();
            }
        }
    }
}
