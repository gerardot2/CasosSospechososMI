using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Common
{
    public class CityModel
{
        [JsonProperty("localidades")]
        public List<City> Localidades { get; set; }
    }
    public class City
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("latitud")]
        public string Latitud { get; set; }

        [JsonProperty("longitud")]
        public string Longitud { get; set; }
    }
}
