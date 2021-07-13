namespace Persistence.Entities
{
    public class Users
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
    }
}