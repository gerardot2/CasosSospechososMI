using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.Family
{
    public class FamilyDataModel
    {
        public FamilyDataModel() { }
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido")]
        public string Apellido { get; set; }

        [JsonPropertyName("dni")]
        public string Dni { get; set; }

        [JsonPropertyName("telefono")]
        public string Telefono { get; set; }

        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }
        [JsonPropertyName("domicilio")]
        public string Domicilio { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("codigo_aspersor")]
        public string CodigoOvitrampa { get; set; }

        [JsonPropertyName("id_localidad")]
        public int IdLocalidad { get; set; }

        [JsonPropertyName("aspersor")]
        public bool Ovitrampa { get; set; }
    }
}


