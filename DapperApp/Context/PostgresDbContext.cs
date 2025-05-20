using Microsoft.EntityFrameworkCore;

namespace DapperApp.Context
{
    public class PostgresDbContext : DatabaseContextBase
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options, httpContextAccessor)
        {
        }
    }
}
