using Microsoft.EntityFrameworkCore;

namespace DapperApp.Context
{
    public class SqlServerDbContext : DatabaseContextBase
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options, httpContextAccessor)
        {
        }
    }
}
