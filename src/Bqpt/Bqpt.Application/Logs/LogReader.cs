////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using BC.Monitors.Kernel;
using Bqpt.Common;
using Bqpt.Infrastructure;
using Dapper;

namespace Bqpt.Application
{
    public class LogReader : ILogReader
    {
        private readonly string _applicationName = AppConstants.AdApplicationName;
        private readonly ISqlConnectionProvider _connectionProvider;

        public LogReader(ISqlConnectionProvider connectionProvider) => _connectionProvider = connectionProvider;

        /// <summary>
        /// Return all LOGs for current application
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ErrorLogDto> AllLogs()
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("MonitorsDb"))
            {
                var sql = @"select * from ErrorLog where ApplicationName = @applicationName order by ErrorLogId desc";

                return conn.Query<ErrorLogDto>(sql, new { applicationName = _applicationName }).ToList();
            }
        }

        /// <summary>
        /// Return the latest LOG for current application
        /// </summary>
        /// <returns></returns>
        public ErrorLogDto LatestLog()
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("MonitorsDb"))
            {
                var sql = @"select top 1 * from ErrorLog where ApplicationName = @applicationName order by ErrorLogId desc";

                var latest = conn.Query<ErrorLogDto>(sql, new { applicationName = _applicationName }).FirstOrDefault();

                return latest is null ? new ErrorLogDto { ErrorLogId = -1 } : latest;
            }
        }

        /// <summary>
        /// Return the Data for the Current Application (LOGS)
        /// </summary>
        /// <returns></returns>
        public ErrorResponseDto LogsDashboard() => new ErrorResponseDto
        {
            Error = LatestLog(),
            ErrorList = AllLogs()
        };

        /// <summary>
        /// Query One specifice Log from DB
        /// </summary>
        /// <param name="errorLogId"></param>
        /// <returns></returns>
        public ErrorLogDto OneLog(int errorLogId)
        {
            using (var conn = _connectionProvider.GetDbSqlConnection("MonitorsDb"))
            {
                var sql = @"select * from ErrorLog where ErrorLogId = @errorLogId";

                return conn.QueryFirstOrDefault<ErrorLogDto>(sql, new { errorLogId });
            }
        }
    }
}