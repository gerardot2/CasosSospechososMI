using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.User
{
    public class HomeDataSupervisorModel : HomeDataModel
    {
        [JsonPropertyName("ultima_familia")]
        public LastFamilyModel UltimaFamilia { get; set; }

        [JsonPropertyName("cantidad_dia")]
        public int? CantidadDia { get; set; }

        [JsonPropertyName("cantidad_mes")]
        public int? CantidadMes { get; set; }
    }
    public class LastFamilyModel
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }
        [JsonPropertyName("fecha_hora")]
        public string FechaHora { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("domicilio")]
        public string Domicilio { get; set; }
        [JsonPropertyName("localidad")]
        public string Localidad { get; set; }
        [JsonPropertyName("resultado")]
        public string Resultado { get; set; }
    }
}
