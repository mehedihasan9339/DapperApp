using Dapper;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DapperApp.Services.Dapper.SQLDapper.Interfaces
{
    public interface IDapper
    {
        DbConnection GetDbconnection();

        // Basic SQL execution
        List<T> FromSql<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text);
        List<T> FromSqlCOB<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text);
        T FromSqlFirstOrDefault<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text);
        T FromSqlFirstOrDefaultCOB<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text);
        T FromSqlSingleOrDefault<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text);

        // Async versions
        Task<List<T>> FromSqlAsync<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text);
        Task<T> FromSqlFirstOrDefaultAsync<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text);
        Task<T> FromSqlSingleOrDefaultAsync<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text);

        // Stored procedure support
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        // Insert/Update via stored procedures
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
