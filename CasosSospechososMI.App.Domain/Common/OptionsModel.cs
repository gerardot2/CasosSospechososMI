using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Common
{
    public class OptionsModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }
}
