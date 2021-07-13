using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.ApiModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions.Window
{
    public class GetAll
    {
        public class Query : IRequest<List<WindowWithoutUser>>
        {
            public string Username { get; set; }
            public bool IncludeExpired { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<WindowWithoutUser>>
        {
            private readonly TrackerContext context;
            private readonly IMapper mapper;

            public Handler(TrackerContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<List<WindowWithoutUser>> Handle(Query request, CancellationToken cancellationToken)
            {
                var windows = await context.Windows.Include(window => window.User)
                                                  .Where(window => window.User.Username == request.Username)
                                                  .Where(window => request.IncludeExpired ? true : (window.StartDate.AddDays(window.NumberOfDays) >= DateTime.Now))
                                                  .OrderByDescending(window => window.WindowId)
                                                  .ToListAsync(cancellationToken);

                var result = mapper.Map<List<WindowWithoutUser>>(windows);

                return result;
            }
        }
    }
}