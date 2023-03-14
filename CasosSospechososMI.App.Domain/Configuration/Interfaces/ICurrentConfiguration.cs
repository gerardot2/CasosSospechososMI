using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Configuration.Interfaces
{
    public interface ICurrentConfiguration
    {
        string ApiUrl { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scope { get; set; }
        string GoogleAndroidClientId { get; set; }
        string GoogleiOSClientId { get; set; }
        string FacebookClientId { get; set; }
        string BranchOfficesMapUrl { get; set; }
        string Web { get; set; }
        string Twitter { get; set; }
        string Facebook { get; set; }
        string Instagram { get; set; }
        string Whatsapp { get; set; }
        string iOSAppStoreId { get; set; }
        string iOSAppStoreName { get; set; }
    }
}
