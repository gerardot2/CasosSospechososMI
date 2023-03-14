using CasosSospechososMI.Domain.Family.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.Family
{
    public class FormModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("tipo_pregunta")]
        public QuestionEnum TipoPregunta { get; set; }

        [JsonPropertyName("numero_pregunta")]
        public int NumeroPregunta { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("opciones")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Opciones { get; set; }
    }
}
