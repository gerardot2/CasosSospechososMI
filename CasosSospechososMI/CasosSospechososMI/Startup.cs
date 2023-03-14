using Acr.UserDialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CasosSospechososMI.Domain.Configuration;
using CasosSospechososMI.Domain.Configuration.Interfaces;
using CasosSospechososMI.Services;
using CasosSospechososMI.Services.Account;
using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.Services.Common;
using CasosSospechososMI.Services.Common.Interfaces;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Loading.ViewModels;
using CasosSospechososMI.UI.Registration.ViewModels;
using CasosSospechososMI.UI.Selector.ViewModels;
using CasosSospechososMI.UseCases.Account;
using CasosSospechososMI.UseCases.Common;
using CasosSospechososMI.UI.Home.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Essentials;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.Services.Family.Interfaces;
using CasosSospechososMI.Services.Family;
using CasosSospechososMI.UI.Sample.ViewModels;
using CasosSospechososMI.UseCases.Sample;
using CasosSospechososMI.Abstractions;
using CasosSospechososMI.UseCases.Family;
using CasosSospechososMI.UI.MyData.ViewModels;
using CasosSospechososMI.UI.Records.ViewModels;
using CasosSospechososMI.UI.Visit.ViewModels;
using CasosSospechososMI.UI.FamilyData.ViewModels;
using CasosSospechososMI.UseCases.Supervisor;
using CasosSospechososMI.Services.Supervisor.Interfaces;
using CasosSospechososMI.Services.Supervisor;
using CasosSospechososMI.UI.Common.ViewModels;
using System.Net.Http;
using FFImageLoading;

namespace CasosSospechososMI
{
    public static class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            //TEMPORAL - CAMBIAR A HostBuilder
            //ConfigureCoreServices(services);
            //ConfigureViewModels(services);
            //

            var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                            })
                            .ConfigureServices((c, x) =>
                            {
                                nativeConfigureServices(c, x);
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o =>
                            {
                                o.DisableColors = true;
                            }))
                            .Build();
            App.ServiceProvider = host.Services;
            //App.DeviceDisplayHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            //App.DeviceDisplayWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            return App.ServiceProvider.GetService<App>();
        }

        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            var appSettingsResourceStream = Assembly.GetAssembly(typeof(Startup)).GetManifestResourceStream("CasosSospechososMI.appsettings.json");
            var jsonString = string.Empty;
            using (var streamReader = new StreamReader(appSettingsResourceStream))
            {
                jsonString = streamReader.ReadToEnd();
            }

            var currentConfiguration = System.Text.Json.JsonSerializer.Deserialize<CurrentConfiguration>(jsonString);
            services.AddSingleton<ICurrentConfiguration>(currentConfiguration);

            services.AddHttpClient();
            services.AddHttpClient("AspersorClient", c =>
            {
                c.BaseAddress = new Uri(currentConfiguration.ApiUrl);
            }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            services.AddTransient<IAuthenticatedService, AuthenticatedService>();

            services.AddSingleton(UserDialogs.Instance);
            ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration { HttpClient = new HttpClient() });
            ConfigureCoreServices(services);
            ConfigureViewModels(services);

            services.AddSingleton<App>();
        }
        private static void ConfigureCoreServices(IServiceCollection services)
        {
            #region Services
            services.AddTransient<IRoutingService, ShellRoutingService>();
            services.AddTransient<ISecureStorageService, SecureStorageService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<IFamilyService, FamilyService>();
            services.AddTransient<ISupervisorService, SupervisorService>();
            #endregion

            #region UseCases
            services.AddTransient<GetActualUser>();
            services.AddTransient<LogOut>();
            services.AddTransient<PostRegistration>();
            services.AddTransient<GetCities>();
            services.AddTransient<GetHomeData>();
            services.AddTransient<GetSampleForm>();
            services.AddTransient<GetActualLocation>();
            services.AddTransient<GetHomeUserData>();
            services.AddTransient<UpdateHomeUserData>();
            services.AddTransient<PostSampleRecord>();
            services.AddTransient<PostVisitRecord>();
            services.AddTransient<GetFamilyData>();
            services.AddTransient<GetHomeUserData>();
            services.AddTransient<GetHomeUserSupervisorData>();
            services.AddTransient<UpdateHomeUserData>();
            services.AddTransient<GetMySamples>();
            services.AddTransient<GetWhatsappNumber>();
            services.AddTransient<GetFamilyDataByCode>();
            services.AddTransient<PostSaveFamilyDataChanges>();
            services.AddTransient<GetSamplesByCode>();
            services.AddTransient<UpdateActualUser>();
            services.AddTransient<GetLastVersion>();
            #endregion
        }
        #region ViewModels
        private static void ConfigureViewModels(IServiceCollection services)
        {
            // add the ViewModel, but as a Transient, which means it will create a new one each time.
            //services.AddTransient<RegistrationViewModel>();
            services.AddTransient<CasosSospechososMI.UI.Home.ViewModels.AboutViewModel>();
            services.AddTransient<BaseViewModel>();
            services.AddTransient<LoadingViewModel>();
            services.AddTransient<CasosSospechososMI.UI.Login.ViewModels.LoginViewModel>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<SelectorViewModel>();
            services.AddTransient<SampleRecordingViewModel>();
            services.AddTransient<MyDataViewModel>();
            services.AddTransient<RecordsViewModel>();
            services.AddTransient<RecordDetailViewModel>();
            services.AddTransient<FamilyDataViewModel>();
            services.AddTransient<VisitListViewModel>();
            services.AddTransient<PlayerViewModel>();
        }
        #endregion
    }
}
