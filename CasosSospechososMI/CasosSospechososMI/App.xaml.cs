
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Models;
using CasosSospechososMI.Services;
using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.UI.Login.Views;
using CasosSospechososMI.UI.Selector.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("AsapBold.ttf", Alias = "AsapBold")]
[assembly: ExportFont("QuicksandBold.otf", Alias = "QuicksandBold")]
namespace CasosSospechososMI
{
    public partial class App : Application
    {
        IAccountService _accountService;
        IAccountService AccountService
                => _accountService ??
                    (_accountService = ServiceProvider.GetService<IAccountService>());
        public static IServiceProvider ServiceProvider { get; set; }
        public static double DeviceDisplayHeight { get; set; }
        public static double DeviceDisplayWidth { get; set; }
        public static string WhatsappNumber { get; set; }
        public static User ActualUser { get;
            set; }
        public static HomeDataModel HomeUserData { get; set; }
        public App()
        {
            InitializeComponent();
            Akavache.Registrations.Start("CasosSospechososMI");
            DependencyService.Register<MockDataStore>();
            var state = AccountService.GetLoginStateAsync(new System.Threading.CancellationToken());
            //_accountService.GetLoginStateAsync(new System.Threading.CancellationToken());
            if (state.Result == Domain.Account.Enums.LoginStateEnum.UserAuthenticated)
            {
                MainPage =  new AppShell();
            }
            else 
            //if (Application.Current.MainPage == null)
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
