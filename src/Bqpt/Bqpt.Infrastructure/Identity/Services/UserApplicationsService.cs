////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC.Identity.Kernel;
using Dapper;

namespace Bqpt.Infrastructure
{
    public class UserApplicationsService : IUserApplicationsService
    {
        private readonly ISqlConnectionProvider _connectionProvider;

        public UserApplicationsService(ISqlConnectionProvider connectionProvider) => _connectionProvider = connectionProvider;

        public async Task<UserInApplicationDto> ApplicationUserName(string userId)
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("IdentityManagerDb"))
            {
                var sql = @"select u.UserName as ApplicationUserName, u.TenantId from [dbo].[User] u where u.Id = @userId";

                var user = await conn.QueryFirstOrDefaultAsync<UserInApplicationDto>(sql, new { userId });

                return user;
            }
        }

        public async Task<IEnumerable<UserInApplicationDto>> MyApplications(string username, string tenantId)
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("IdentityManagerDb"))
            {
                var sql = @"select t.ApplicationName,
                                    u.Id as ApplicationUserId,
                                    t.TenantId,
                                    COALESCE (NULLIF (LOWER(t.ApplicationUrl), N''), N'https://localhost/') as ApplicationUrl
                            from ApplicationTenant t
                                    inner join [dbo].[User] u on u.TenantId = t.TenantId and u.UserName = @username and u.EmailConfirmed = 1 and t.TenantId <> @tenantId and t.IsActive = 1";

                return await conn.QueryAsync<UserInApplicationDto>(sql, new { username, tenantId });
            }
        }

        public async Task<string> MyApplicationsCard(string username, string tenantId)
        {
            var card = new StringBuilder();
            var apps = await MyApplications(username, tenantId);

            if (!apps.Any()) return "No Applications registered for Current User";

            foreach (var app in apps.Where(a => !a.ApplicationUrl.Contains("localhost")))
            {
                var uri = new Uri($"{app.ApplicationUrl}account/connector/{app.ApplicationUserId}");
                var likUrlStyle = app.ApplicationUrl.Contains("localhost") ? "text-danger" : "text-primary";

                card.Append($"<a class=\"dropdown-item {likUrlStyle}\" href=\"{uri}\" target=\"_blank\"><span class=\"fas fa-database mr-2\" aria-hidden=\"true\"></span>{app.ApplicationName}</a>");
            }

            return card.ToString();
        }

        public async Task<string> UserIdentityCard(string userId)
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("IdentityManagerDb"))
            {
                var sql = @"select
                                u.FirstName,
                                u.LastName,
                                u.PhoneNumber,
                                g.Name as PrimaryGroup
                            from
                                [dbo].[User] u
                            inner join
                                [dbo].[UserGroup] ug on u.Id = ug.ApplicationUserId and u.Id = @userId
                            inner join
                                [dbo].[Group] g on ug.ApplicationGroupId = g.ApplicationGroupId";

                var applicationUser = await conn.QueryFirstOrDefaultAsync<ApplicationUserDto>(sql, new { userId });

                return $@"<small>
                            <span class='fa fa-user-circle mr-1' aria-hidden='true'></span>{applicationUser.LastName},  {applicationUser.FirstName}
                            <br />
                            <span class='fa fa-users mr-1' aria-hidden='true'></span>{applicationUser.PrimaryGroup}
                            <br />
                            <span class='fa fa-phone-square mr-1' aria-hidden='true'></span>{applicationUser.PhoneNumber}
                            <br />
                            <hr />
                            <strong>
                            <span class='fa fa-calendar mr-1' aria-hidden='true'></span>Last Login Date:
                            </strong>
                            <br />
                            {applicationUser.LastLoginDate}</small>".ToSecureHash();
            }
        }
    }
}