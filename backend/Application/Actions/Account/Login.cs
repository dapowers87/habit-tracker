using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Security;
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
            public string Password { get; set; }
            public string IpAddress { get; set; }
        }

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly AuthenticationSettings config;
            private readonly ILogger<Login> logger;
            private readonly IJWTHandler jwtHandler;
            private readonly SbrContext context;

            public Handler(IOptions<AuthenticationSettings> config, ILogger<Login> logger, IJWTHandler jwt, SbrContext context)
            {
                this.jwtHandler = jwt;
                this.context = context;
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

                if (request.Password == config.Password)
                {
                    if(loginAudit.LoginAttemptDate.AddDays(1) <= DateTime.Now)
                    {
                        loginAudit.FailCount = 0;
                    }

                    if(loginAudit.FailCount <= config.MaxAttempts)
                    {
                        result = jwtHandler.CreateToken();
                        loginAudit.FailCount = 0;
                    }
                }
                else
                {
                    loginAudit.FailCount++;
                }

                await context.SaveChangesAsync(cancellationToken);

                return result;
            }
        }
    }
}