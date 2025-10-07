////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Bqpt.Infrastructure
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId => HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.GetUserId() : Guid.Empty.ToString();

        public bool IsAuthenticated => HttpContext.Current.User.Identity.IsAuthenticated;

        public string UserName => HttpContext.Current.User.Identity.GetUserName();

        public bool HasPermission(string role) => HttpContext.Current.User.IsInRole(role);

        public IEnumerable<string> UserPermissions() => throw new NotImplementedException("Developer's Implementation of Roles Store");
    }
}