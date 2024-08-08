﻿namespace Shared.Models
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public double ExpiresInMinutes { get; set; } = 60;
    }
}
