using System.Threading;
using System.Threading.Tasks;
using Domain.ApiModels;
using MediatR;
using Persistence;

namespace Application.Actions.Window
{
    public class Create
    {
        public class Command : IRequest<int>
        {
            public CreateWindowModel Model { get; set; }
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
                var newWindow = new Persistence.Entities.Window
                {
                    StartDate = request.Model.StartDate,
                    NumberOfDays = request.Model.NumberOfDays,
                    NumberOfCheatDays = request.Model.NumberOfCheatDays
                };

                await context.Windows.AddAsync(newWindow, cancellationToken);

                var saveResult = await context.SaveChangesAsync(cancellationToken) > 0;

                var result = saveResult ? newWindow.WindowId : -1;

                return result;
            }
        }
    }
}