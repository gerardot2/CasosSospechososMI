using CasosSospechososMI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CasosSospechososMI.UseCases.Account
{
    public class GetActualLocation
    {
        
        public async Task<Domain.Common.Location> Invoke()
        {
            
            var request = new GeolocationRequest(GeolocationAccuracy.High);
            var location1 = new Xamarin.Essentials.Location();
            if (Device.RuntimePlatform == Device.Android)
            {
                location1 = await Geolocation.GetLocationAsync(request);
            }
            var ubicacion = new Domain.Common.Location();
            if (location1 != null)
            {
                ubicacion.Latitude = location1.Latitude.ToString();
                ubicacion.Longitude = location1.Longitude.ToString();
            }
            return ubicacion;
        }
    }
}
