using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Account
{
    public class GetUsernames
    {
        public class Query : IRequest<List<string>>
        {

        }

        public class Handler : IRequestHandler<Query, List<string>>
        {
            private readonly TrackerContext context;


            public Handler(TrackerContext context)
            {
                this.context = context;
            }

            public async Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await context.Users.Select(u => u.Username).ToListAsync(cancellationToken);

                return users;
            }
        }
    }
}