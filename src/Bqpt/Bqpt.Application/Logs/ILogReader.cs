////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using BC.Monitors.Kernel;

namespace Bqpt.Application
{
    public interface ILogReader
    {
        ErrorResponseDto LogsDashboard();

        IEnumerable<ErrorLogDto> AllLogs();

        ErrorLogDto OneLog(int errorLogId);

        ErrorLogDto LatestLog();
    }
}