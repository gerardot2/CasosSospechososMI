using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.DTOs
{
    public class TokenDTO
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        public DateTime ExpiresAt { get; set; }
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("apellido")]
        public string Apellido { get; set; }
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }
        [JsonPropertyName("role")]
        public int? RoleId { get; set; }
    }
}
