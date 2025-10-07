////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Configuration;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace Bqpt.Infrastructure
{
    public class SqlConnectionProvider : ISqlConnectionProvider
    {
        public SqlConnection GetDbSqlConnection(string name) => new SqlConnection(ConfigurationManager.ConnectionStrings[name].ToString());

        public OracleConnection GetDbOracleConnection(string name) => new OracleConnection(ConfigurationManager.ConnectionStrings[name].ToString());
    }
}