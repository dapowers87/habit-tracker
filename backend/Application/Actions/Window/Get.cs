using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.ApiModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Window
{
    public class Get
    {
        public class Query : IRequest<WindowWithoutUser>
        {
            public string Username { get; set; }
            public int WindowId { get; set; }
        }

        public class Handler : IRequestHandler<Query, WindowWithoutUser>
        {
            private readonly TrackerContext context;
            private readonly IMapper mapper;

            public Handler(TrackerContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<WindowWithoutUser> Handle(Query request, CancellationToken cancellationToken)
            {
                var window = await context.Windows.Include(w => w.User).FirstOrDefaultAsync(window => window.WindowId == request.WindowId);

                if(window.User.Username != request.Username)
                {
                    return null;
                }

                var result = mapper.Map<WindowWithoutUser>(window);

                return result;
            }
        }
    }
}