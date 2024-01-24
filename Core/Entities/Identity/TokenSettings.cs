namespace Core.Entities.Identity
{
    public class TokenSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

}

