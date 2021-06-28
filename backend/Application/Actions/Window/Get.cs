using System.Threading;
using System.Threading.Tasks;
using Domain.ApiModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Window
{
    public class Get
    {
        public class Query : IRequest<Persistence.Entities.Window>
        {
            public int WindowId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Persistence.Entities.Window>
        {
            private readonly TrackerContext context;
            public Handler(TrackerContext context)
            {
                this.context = context;
            }

            public async Task<Persistence.Entities.Window> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await context.Windows.FirstOrDefaultAsync(window => window.WindowId == request.WindowId);

                return result;
            }
        }
    }
}