using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Security;
using Domain.ApiModels;
using Domain.Configuration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Entities;

namespace Application.Actions.Account
{
    public class Login
    {
        public class Command : IRequest<string>
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string IpAddress { get; set; }
        }

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly AuthenticationSettings config;
            private readonly ILogger<Login> logger;
            private readonly IJWTHandler jwtHandler;
            private readonly TrackerContext context;
            private readonly IPasswordHasher passwordHasher;

            public Handler(IOptions<AuthenticationSettings> config, ILogger<Login> logger, IJWTHandler jwt, TrackerContext context, IPasswordHasher passwordHasher)
            {
                this.jwtHandler = jwt;
                this.context = context;
                this.passwordHasher = passwordHasher;
                this.logger = logger;
                this.config = config.Value;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = string.Empty;
                
                var loginAudit = (LoginAudit)null;

                var prevAudit = await context.LoginAudits.FirstOrDefaultAsync(login => login.IpAddress == request.IpAddress, cancellationToken);

                if(prevAudit == null)
                {
                    var newLoginAudit = new LoginAudit
                    {
                        IpAddress = request.IpAddress,
                        LoginAttemptDate = DateTime.Now
                    };

                    await context.LoginAudits.AddAsync(newLoginAudit, cancellationToken);
                    loginAudit = newLoginAudit;
                }
                else 
                {
                    loginAudit = prevAudit;
                }

                if(loginAudit.LoginAttemptDate.AddMinutes(5) <= DateTime.Now)
                {
                    loginAudit.FailCount = 0;
                }

                loginAudit.LoginAttemptDate = DateTime.Now;

                if(loginAudit.FailCount <= config.MaxAttempts)
                {
                    var user = await context.Users.FirstOrDefaultAsync(user => user.Username == request.Username.ToLower().Trim());

                    if(user == null)
                    {
                        logger.LogInformation($"User '{request.Username}' not found");
                        loginAudit.FailCount++;
                        await context.SaveChangesAsync(cancellationToken);

                        return result;
                    }

                    if (passwordHasher.CheckPassword(request.Password, user.PasswordHash))
                    {
                        result = jwtHandler.CreateToken(user.Username, user.IsAdmin);
                        loginAudit.FailCount = 0;
                    }
                    else
                    {
                        loginAudit.FailCount++;
                    }
                }
                else
                {
                    loginAudit.FailCount++;
                    result = null;
                }

                await context.SaveChangesAsync(cancellationToken);

                return result;
            }
        }
    }
}