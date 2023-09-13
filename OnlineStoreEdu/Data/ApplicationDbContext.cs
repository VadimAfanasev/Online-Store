using Microsoft.EntityFrameworkCore;
using OnlineStoreEdu.Models;

namespace OnlineStoreEdu.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Category { get; set; }
    }
}
