using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using CasosSospechososMI;
using CasosSospechososMI.Domain.Account.Enums;
using CasosSospechososMI.Domain.Configuration.Interfaces;
using CasosSospechososMI.Droid;
using CasosSospechososMI.Services;
using CasosSospechososMI.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    
    //IAccountService _accountService;
    //IAccountService AccountService
    //        => _accountService ??
    //            (_accountService = ServiceContainer.Resolve<IAccountService>());
    public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
    {
        base.OnCreate(savedInstanceState, persistentState);
    }

    // Launches the startup task
    protected override void OnResume()
    {
        base.OnResume();
        Task startupWork = new Task(() => { SimulateStartup(); });
        startupWork.Start();
    }
    protected override void OnPause()
    {
        base.OnPause();
        //-- Comentado por error de comp en VS2022
        //SetContentView(Resource.Drawable.splash_procev);
    }

    // Simulates background work that happens behind the splash screen
    async void SimulateStartup()
    {
        
        await Task.Delay(3000); // Simulate a bit of startup work.

        
        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }
    public override void OnBackPressed() { }
}