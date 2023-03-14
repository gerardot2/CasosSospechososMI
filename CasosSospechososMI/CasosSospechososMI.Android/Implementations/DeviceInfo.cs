using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using CasosSospechososMI.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(CasosSospechososMI.Droid.Implementations.AndroidDeviceInfo))]
namespace CasosSospechososMI.Droid.Implementations
{
    public class AndroidDeviceInfo : IDeviceInfo
    {
        public string GetPhoneNumber()
        {
            var tMgr = (TelephonyManager)Android.App.Application.Context.ApplicationContext.GetSystemService(Android.Content.Context.TelephonyService);
            return tMgr.Line1Number;
        }
    }
}