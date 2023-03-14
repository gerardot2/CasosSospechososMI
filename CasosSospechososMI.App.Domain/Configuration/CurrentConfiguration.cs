using CasosSospechososMI.Domain.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Configuration
{
    public class CurrentConfiguration : ICurrentConfiguration
    {
        public string ApiUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string GoogleAndroidClientId { get; set; }
        public string GoogleiOSClientId { get; set; }
        public string FacebookClientId { get; set; }
        public string BranchOfficesMapUrl { get; set; }
        public string Web { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Whatsapp { get; set; }
        public string iOSAppStoreId { get; set; }
        public string iOSAppStoreName { get; set; }
    }
}
