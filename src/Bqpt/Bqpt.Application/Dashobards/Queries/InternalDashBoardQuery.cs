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
    public class InternalDashBoardQuery : IRequest<AdminDashboardResponseDto>
    {
        public class Handler : IRequestHandler<InternalDashBoardQuery, AdminDashboardResponseDto>
        {
            public Task<AdminDashboardResponseDto> Handle(InternalDashBoardQuery request, CancellationToken cancellationToken) => Task.FromResult(new AdminDashboardResponseDto());
        }
    }
}