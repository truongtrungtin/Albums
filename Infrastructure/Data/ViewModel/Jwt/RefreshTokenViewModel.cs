using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Infrastructure.Data.ViewModel
{
    public class RefreshTokenViewModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.Now >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIP { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIP { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
