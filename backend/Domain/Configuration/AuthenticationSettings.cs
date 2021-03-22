namespace Domain.Configuration
{
    public class AuthenticationSettings
    {
        public string Password { get; set; }
        public string AuthKey { get; set; }
        public int TokenExpirationDays { get; set; }
        public int MaxAttempts { get; set; }
    }
}