using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.Common
{
    public class ResponseGeneric
    {
        [JsonPropertyName("codigo")]
        public string codigo { get; set; }
        [JsonPropertyName("error")]
        public string error { get; set; }
        [JsonPropertyName("mensaje")]
        public string mensaje { get; set; }
        [JsonPropertyName("id")]
        public string id { get; set; }

    }
}
