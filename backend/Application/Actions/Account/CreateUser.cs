using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Security;
using Domain.ApiModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

namespace Application.Actions.Account
{
    public class CreateUser
    {
        public class Command : IRequest<CreateUserResponse>
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
        }

        public class Handler : IRequestHandler<Command, CreateUserResponse>
        {
            private readonly IPasswordHasher passwordHasher;
            private readonly TrackerContext context;

            public Handler(IPasswordHasher passwordHasher, TrackerContext context)
            {
                this.passwordHasher = passwordHasher;
                this.context = context;
            }

            public async Task<CreateUserResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var usersInDb = context.Users.Any();

                if(!usersInDb)
                {
                    request.IsAdmin = true;
                }

                var userExists = await context.Users.FirstOrDefaultAsync(user => user.Username == request.Username) != null;

                if(userExists)
                {
                    return new CreateUserResponse
                    {
                        Success = false,
                        ErrorMessage = $"User '{request.Username}' already exists"
                    };
                }

                var user = new User
                {
                    Username = request.Username.ToLower().Trim(),
                    PasswordHash = passwordHasher.HashPassword(request.Password),
                    IsAdmin = request.IsAdmin
                };

                await context.Users.AddAsync(user, cancellationToken);
                
                await context.SaveChangesAsync(cancellationToken);

                return new CreateUserResponse
                {
                    Success = true
                };
            }
        }
    }
}