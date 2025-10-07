////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC.Identity.Kernel;
using Dapper;

namespace Bqpt.Infrastructure
{
    public class IdentityManagerServices : IIdentityManagerServices
    {
        private readonly ISqlConnectionProvider _connectionProvider;

        public IdentityManagerServices(ISqlConnectionProvider connectionProvider) => _connectionProvider =
            connectionProvider;

        public async Task<IEnumerable<ApplicationGroupDto>> GetApplicationRoles(string tenantId)
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("IdentityManagerDb"))
            {
                var sql = @"select  g.Name,
		                            g.ApplicationGroupId,
		                            g.IsDefault
                            from	[dbo].[Group] g
                            where	g.TenantId = @tenantId and g.Name <> 'Developers'";

                var groups = await conn.QueryAsync<ApplicationGroupDto>(sql, new { tenantId });

                return groups is null && !groups.Any() ? Enumerable.Empty<ApplicationGroupDto>() : groups;
            }
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetApplicationUsers(string tenantId, bool userStatus = true)
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("IdentityManagerDb"))
            {
                var sql = @"select  u.Id as ApplicationUserId,
		                            u.FirstName,
		                            u.LastName,
		                            g.Name as UserGroup,
		                            u.EmployeeNumber,
		                            u.Email,
		                            u.EmailConfirmed as IsActive,
		                            u.PhoneNumber,
		                            u.LastConnectedIpAddress,
		                            u.LastLoginDate
                        from        [dbo].[User] u
                        inner join  [dbo].[UserGroup] ug
	                        on      u.Id = ug.ApplicationUserId  and u.TenantId = @tenantId
                        inner join  [dbo].[Group] g
	                        on      g.ApplicationGroupId = ug.ApplicationGroupId";

                var users = await conn.QueryAsync<ApplicationUserDto>(sql, new { tenantId });

                return users.Where(u => u.IsActive == userStatus).ToList();
            }
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetApplicationUsersInRole(string tenantId, string role)
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("IdentityManagerDb"))
            {
                var sql = @"select  u.Id, u.TenantId,
                                    u.FirstName,
                                    u.LastName,
                                    u.LastConnectedIpAddress,
                                    u.LastLoginDate,
                                    u.RegistrationDate,
                                    u.Email,
                                    u.UserName,
                                    u.CreatedBy,
                                    u.CreatedOn,
                                    u.PhoneNumber,
                                    g.[Name] AS UserGroup,
                                    u.EmailConfirmed AS IsActive,
                                    ug.ApplicationGroupId as GroupId,
                                    u.EmployeeNumber
                        from        dbo.[Group] g
                        inner join  dbo.UserGroup ug
                            on      g.ApplicationGroupId = ug.ApplicationGroupId and g.Name = @role
                        inner join  dbo.[User] U
                            on      ug.ApplicationUserId =  u.Id and u.TenantId = @tenantId";

                var users = await conn.QueryAsync<ApplicationUserDto>(sql, new { role, tenantId });

                return users is null && !users.Any()
                    ? Enumerable.Empty<ApplicationUserDto>()
                    : users.ToList();
            }
        }
    }
}