using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Common
{
    public class Location
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsComplete
        {
            get { return !string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude); }
        }
    }
}
