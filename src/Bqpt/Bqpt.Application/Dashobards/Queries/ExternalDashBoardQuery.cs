////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Bqpt.Application
{
    public class ExternalDashBoardQuery : IRequest<AdminDashboardResponseDto>
    {
        public class Handler : IRequestHandler<ExternalDashBoardQuery, AdminDashboardResponseDto>
        {
            public Task<AdminDashboardResponseDto> Handle(ExternalDashBoardQuery request, CancellationToken cancellationToken) => Task.FromResult(new AdminDashboardResponseDto { });
        }
    }
}