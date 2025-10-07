////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bqpt.Infrastructure
{
    public interface IUserApplicationsService
    {
        Task<IEnumerable<UserInApplicationDto>> MyApplications(string username, string tenantId);

        Task<string> MyApplicationsCard(string username, string tenantId);

        Task<string> UserIdentityCard(string userId);

        Task<UserInApplicationDto> ApplicationUserName(string userId);
    }
}