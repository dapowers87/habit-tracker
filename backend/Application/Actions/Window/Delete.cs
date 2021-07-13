using System.Threading;
using System.Threading.Tasks;
using Domain.ApiModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Window
{
    public class Delete
    {
        public class Command : IRequest<int>
        {
            public string Username { get; set; }
            public int WindowId { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly TrackerContext context;
            public Handler(TrackerContext context)
            {
                this.context = context;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var window = await context.Windows.Include(w => w.User).FirstOrDefaultAsync(window => window.WindowId == request.WindowId);

                if(window == null)
                {
                    return -1;
                }
                else if(window.User.Username != request.Username)
                {
                    return -1;
                }
                
                context.Windows.Remove(window);

                var saveResult = await context.SaveChangesAsync(cancellationToken) > 0;

                var result = saveResult ? 1 : 0;

                return result;
            }
        }
    }
}