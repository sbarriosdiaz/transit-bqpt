////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Threading.Tasks;
using BC.Identity.Kernel;

namespace Bqpt.Infrastructure
{
    public interface IIdentityManagerServices
    {
        Task<IEnumerable<ApplicationUserDto>> GetApplicationUsers(string tenantId, bool userStatus = true);

        Task<IEnumerable<ApplicationUserDto>> GetApplicationUsersInRole(string tenantId, string role);

        Task<IEnumerable<ApplicationGroupDto>> GetApplicationRoles(string tenantId);
    }
}