////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace Bqpt.Infrastructure
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserName { get; }
        bool IsAuthenticated { get; }

        bool HasPermission(string role);

        IEnumerable<string> UserPermissions();
    }
}