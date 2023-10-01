using Microsoft.EntityFrameworkCore;
using PracticeAPI4.Models;

namespace PracticeAPI4.DB
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext>
            options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
    }
}
