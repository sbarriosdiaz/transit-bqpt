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
    public class AdminDashBoardQuery : IRequest<AdminDashboardResponseDto>
    {
        public class Handler : IRequestHandler<AdminDashBoardQuery, AdminDashboardResponseDto>
        {
            public Task<AdminDashboardResponseDto> Handle(AdminDashBoardQuery request, CancellationToken cancellationToken) => Task.FromResult(new AdminDashboardResponseDto { });
        }
    }
}