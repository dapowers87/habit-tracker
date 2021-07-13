using System.Threading;
using System.Threading.Tasks;
using Domain.ApiModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Window
{
    public class Create
    {
        public class Command : IRequest<int>
        {
            public string Username { get; set; }
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
                var user = await context.Users.FirstOrDefaultAsync(user => user.Username == request.Username);

                if(user == null)
                {
                    return 2;
                }

                var newWindow = new Persistence.Entities.Window
                {
                    WindowName = request.Model.WindowName,
                    StartDate = request.Model.StartDate.Date,
                    NumberOfDays = request.Model.NumberOfDays,
                    NumberOfCheatDays = request.Model.NumberOfCheatDays,
                    User = user
                };

                await context.Windows.AddAsync(newWindow, cancellationToken);

                var saveResult = await context.SaveChangesAsync(cancellationToken) > 0;

                var result = saveResult ? newWindow.WindowId : -1;

                return result;
            }
        }
    }
}