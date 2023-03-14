

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.User
{
    public class HomeDataModel
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }

        [JsonPropertyName("aspersor")]
        public string Ovitrampa { get; set; }

        [JsonPropertyName("muestras_registradas")]
        public long MuestrasRegistradas { get; set; }

        [JsonPropertyName("fecha_ultima_muestra")]
        public string FechaUltimaMuestra { get; set; }

        [JsonPropertyName("fecha_proxima_muestra")]
        public string FechaProximaMuestra { get; set; }

        [JsonPropertyName("dias_para_proxima_muestra")]
        public int DiasParaProximaMuestra { get; set; }

        [JsonPropertyName("mensaje_dias")]
        public string MensajeDias { get; set; }

        [JsonPropertyName("aviso_cabecera")]
        public string AvisoCabecera { get; set; }

        [JsonPropertyName("aviso_texto")]
        public string AvisoTexto { get; set; }
        
    }
}


