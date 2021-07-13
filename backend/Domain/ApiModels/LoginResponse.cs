namespace Domain.ApiModels
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
        public bool LockedOut { get; set; }
    }
}