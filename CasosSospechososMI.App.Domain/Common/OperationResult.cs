using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.Common
{
    [Serializable]
    public class OperationResult<T>
    {
        public OperationResult()
        {
        }
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }
        [JsonPropertyName("numero")]
        public string Numero { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }

    }
}
