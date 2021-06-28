using System.Threading;
using System.Threading.Tasks;
using Domain.ApiModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Window
{
    public class UpdateCheatDays
    {
        public class Command : IRequest<int>
        {
            public int WindowId { get; set; }
            public int NewCheatDaysUsed { get; set; }
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
                var window = await context.Windows.FirstOrDefaultAsync(window => window.WindowId == request.WindowId);

                if(window == null)
                {
                    return -1;
                }

                window.NumberOfCheatDaysUsed = request.NewCheatDaysUsed;

                var saveResult = await context.SaveChangesAsync(cancellationToken) > 0;

                var result = saveResult ? 1 : 0;

                return result;
            }
        }
    }
}