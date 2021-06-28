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
                var result = await context.Windows.Where(window => request.IncludeExpired ? true : (window.StartDate.AddDays(window.NumberOfDays) >= DateTime.Now))
                                                  .OrderByDescending(window => window.WindowId)
                                                  .ToListAsync(cancellationToken);

                return result;
            }
        }
    }
}