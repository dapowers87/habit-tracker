using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Window
{
    public class GetAll
    {
        public class Query : IRequest<List<Persistence.Entities.Window>>
        {
            public string Username { get; set; }
            public bool IncludeExpired { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Persistence.Entities.Window>>
        {
            private readonly TrackerContext context;
            public Handler(TrackerContext context)
            {
                this.context = context;
            }

            public async Task<List<Persistence.Entities.Window>> Handle(Query request, CancellationToken cancellationToken)
            {
                Console.WriteLine(request.Username);
                foreach(var window in context.Windows.Include(w => w.User))
                {
                    Console.WriteLine($"ID: {window.WindowId}\tUsername: {window.User?.Username}");
                }
                var result = await context.Windows.Include(window => window.User)
                                                  .Where(window => window.User.Username == request.Username)
                                                  .Where(window => request.IncludeExpired ? true : (window.StartDate.AddDays(window.NumberOfDays) >= DateTime.Now))
                                                  .OrderByDescending(window => window.WindowId)
                                                  .ToListAsync(cancellationToken);



                return result;
            }
        }
    }
}