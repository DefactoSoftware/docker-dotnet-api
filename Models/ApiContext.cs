using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace apicore.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
           base.OnModelCreating(builder);
           builder.HasPostgresExtension("uuid-ossp");
         }

        public DbSet<People> People { get; set; }
    }
}
