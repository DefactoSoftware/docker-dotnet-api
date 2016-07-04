using Microsoft.EntityFrameworkCore;

namespace apicore.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) {}

        public DbSet<People> People { get; set; }
    }
}