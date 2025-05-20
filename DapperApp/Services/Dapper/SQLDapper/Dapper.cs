using Dapper;
using DapperApp.Services.Dapper.SQLDapper.Interfaces;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace DapperApp.Services.Dapper.SQLDapper
{
    public class Dapper : IDapper, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        private readonly string _databaseUsed;

        public Dapper(IConfiguration config)
        {
            _config = config;
            _databaseUsed = _config["DatabaseUsed"];

            _connectionString = _databaseUsed switch
            {
                "SQL" => _config.GetConnectionString("SqlServer"),
                "POSTGRES" => _config.GetConnectionString("Postgres"),
                _ => throw new InvalidOperationException("Unsupported or missing DatabaseUsed configuration")
            };
        }

        private IDbConnection CreateConnection()
        {
            return _databaseUsed switch
            {
                "SQL" => new SqlConnection(_connectionString),
                "POSTGRES" => new NpgsqlConnection(_connectionString),
                _ => throw new InvalidOperationException("Unsupported database type.")
            };
        }

        public void Dispose()
        {
            // Optional
        }

        public async Task<List<T>> FromSqlAsync<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text)
        {
            using var db = (DbConnection)CreateConnection(); // Cast to DbConnection
            await db.OpenAsync();
            var query = SqlQuery.ToString();
            var result = await db.QueryAsync<T>(query, commandType: commandType);
            return result.ToList();
        }


        public List<T> FromSql<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text)
        {
            using var db = CreateConnection();
            var result = db.Query<T>(SqlQuery.ToString(), commandType: commandType);
            return result.ToList();
        }

        public List<T> FromSqlCOB<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text)
        {
            using var db = CreateConnection();
            var timeout = TimeSpan.FromSeconds(36000);
            var result = db.Query<T>(SqlQuery.ToString(), commandType: commandType, commandTimeout: (int)timeout.TotalSeconds);
            return result.ToList();
        }

        public async Task<T> FromSqlFirstOrDefaultAsync<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text)
        {
            using var db = CreateConnection();
            var result = await db.QueryFirstOrDefaultAsync<T>(SqlQuery.ToString(), commandType: commandType);
            return result;
        }

        public T FromSqlFirstOrDefault<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text)
        {
            using var db = CreateConnection();
            return db.QueryFirstOrDefault<T>(SqlQuery.ToString(), commandType: commandType);
        }

        public T FromSqlFirstOrDefaultCOB<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text)
        {
            using var db = CreateConnection();
            var timeout = TimeSpan.FromSeconds(36000);
            return db.QueryFirstOrDefault<T>(SqlQuery.ToString(), commandType: commandType, commandTimeout: (int)timeout.TotalSeconds);
        }

        public async Task<T> FromSqlSingleOrDefaultAsync<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text)
        {
            using var db = CreateConnection();
            return await db.QuerySingleOrDefaultAsync<T>(SqlQuery.ToString(), commandType: commandType);
        }

        public T FromSqlSingleOrDefault<T>(FormattableString SqlQuery, CommandType commandType = CommandType.Text)
        {
            using var db = CreateConnection();
            return db.QuerySingleOrDefault<T>(SqlQuery.ToString(), commandType: commandType);
        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = CreateConnection();
            return db.Execute(sp, parms, commandType: commandType);
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = CreateConnection();
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = CreateConnection();
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public DbConnection GetDbconnection()
        {
            return (DbConnection)CreateConnection();
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = CreateConnection();
            db.Open();
            using var tran = db.BeginTransaction();
            try
            {
                var result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                tran.Commit();
                return result;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = CreateConnection();
            db.Open();
            using var tran = db.BeginTransaction();
            try
            {
                var result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                tran.Commit();
                return result;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
