using System.Threading;
using System.Threading.Tasks;
using Domain.ApiModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Window
{
    public class Update
    {
        public class Command : IRequest<int>
        {
            public UpdateWindowModel Model { get; set; }
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
                var window = await context.Windows.FirstOrDefaultAsync(window => window.WindowId == request.Model.WindowId);

                if(window == null)
                {
                    return -1;
                }

                window.WindowName = request.Model.WindowName;
                window.StartDate = request.Model.StartDate;
                window.NumberOfDays = request.Model.NumberOfDays;
                window.NumberOfCheatDays = request.Model.NumberOfCheatDays;
                window.NumberOfCheatDaysUsed = request.Model.NumberOfCheatDaysUsed;

                var saveResult = await context.SaveChangesAsync(cancellationToken);

                var result = saveResult == 1 || saveResult == 0 ? 1 : 0;

                return result;
            }
        }
    }
}