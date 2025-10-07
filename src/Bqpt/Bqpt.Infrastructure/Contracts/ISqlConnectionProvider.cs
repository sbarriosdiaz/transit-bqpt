////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace Bqpt.Infrastructure
{
    public interface ISqlConnectionProvider
    {
        SqlConnection GetDbSqlConnection(string name);

        OracleConnection GetDbOracleConnection(string name);
    }
}