using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CasosSospechososMI.Abstractions;
using CasosSospechososMI.Droid.Implementations;
using Acr.UserDialogs;
using AndroidX.AppCompat.App;
using Xamarin.Forms;
using CasosSospechososMI.Models;
using CasosSospechososMI.UseCases.Account;
using System.Threading;
using FFImageLoading.Forms.Platform;

namespace CasosSospechososMI.Droid
{
    [Activity(Label = "CasosSospechososMI", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | 
        ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //MobileBarcodeScanner scanner;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            Rg.Plugins.Popup.Popup.Init(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);
            CachedImageRenderer.InitImageViewHandler();
            //scanner = new MobileBarcodeScanner();
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            //LoadApplication(new App());
            //Inicia servicios con Startup
            UserDialogs.Init(this);
            MessagingCenter.Subscribe<GenericMessage>(this, "Shutdown", OnShutdown);
            MessagingCenter.Subscribe<GenericMessage>(this, "OnlyShutdown", OnlyOnShutdown);
            LoadApplication(Startup.Init(ConfigureServices));
        }

        private void OnlyOnShutdown(GenericMessage obj)
        {

            Finish();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private async void OnShutdown(GenericMessage obj)
        {
            var logOut = App.ServiceProvider.GetService<LogOut>();
            
            var cTS = new CancellationTokenSource();
            await logOut.Invoke(cTS.Token);
            Finish();
        }
        void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            //if we want to inject platform specific implementation
            services.AddSingleton<IDeviceInfo, AndroidDeviceInfo>();
            services.AddSingleton<IDeviceInfo, AndroidDeviceInfo>();
        }
        
    }
}