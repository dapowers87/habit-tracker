namespace Domain.ApiModels
{
    public class CreateAdminUserRequest: CreateUserRequest
    {
        public bool IsAdmin { get; set; }
    }
}