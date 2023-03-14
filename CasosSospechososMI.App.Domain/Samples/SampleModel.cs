using CasosSospechososMI.Domain.Family.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.Samples
{
    public class SampleModel
    {
        public SampleModel() { }
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("res1")]
        public SampleUnit Res1 { get; set; }

        [JsonPropertyName("res2")]
        public SampleUnit Res2 { get; set; }

        [JsonPropertyName("res3")]
        public SampleUnit Res3 { get; set; }

        [JsonPropertyName("res4")]
        public SampleUnit Res4 { get; set; }

        [JsonPropertyName("imagen")]
        public string Imagen { get; set; }

        [JsonPropertyName("fecha_carga")]
        public string FechaCarga { get; set; }
        [JsonPropertyName("comentario")]
        public string Comentario { get; set; }
        
        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        [JsonPropertyName("resultado")]
        public string Resultado { get; set; }
        public string EstadoColor
        {
            get
            {
                switch (Estado)
                {
                    case "Pendiente":
                        return "#e61610";
                    default:
                        return "#35424a";
                }
            }
        }
        public string ResultadoFormat
        {
            get
            {
                if (string.IsNullOrEmpty(Resultado)) return "--";
                return Resultado;
            }
        }

        public string FechaCargaFormatted
        {
            get
            {
                try
                {
                    var dto = DateTimeOffset.Parse(FechaCarga);
                    return dto.Date.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    return "Error obteniendo fecha";
                }
                
            }
        }
        public bool HasRes1
        {
            get
            {
                return !string.IsNullOrEmpty(Res1.Tipo.ToString())
                    && !string.IsNullOrEmpty(Res1.Descripcion);
            }
        }public bool HasRes2
        {
            get
            {
                return !string.IsNullOrEmpty(Res2.Tipo.ToString())
                    && !string.IsNullOrEmpty(Res2.Descripcion);
            }
        }public bool HasRes3
        {
            get
            {
                return !string.IsNullOrEmpty(Res3.Tipo.ToString())
                    && !string.IsNullOrEmpty(Res3.Descripcion);
            }
        }
        public bool HasRes4
        {
            get
            {
                return !string.IsNullOrEmpty(Res4.Tipo.ToString()) && !string.IsNullOrEmpty(Res4.Descripcion);
            }
        }
        public bool HasComment
        {
            get
            {
                var res = !string.IsNullOrWhiteSpace(Comentario);
                return res;
            }
        }
        public bool HasPhoto
        {
            get
            {
                return !string.IsNullOrEmpty(Imagen);
            }
        }
        
    }

    public class SampleUnit
    {
        [JsonPropertyName("tipo")]
        public QuestionEnum Tipo { get; set; }

        [JsonPropertyName("descr")]
        public string Descripcion { get; set; }
        [JsonPropertyName("pregunta")]
        public string Pregunta { get; set; }
    }
}
