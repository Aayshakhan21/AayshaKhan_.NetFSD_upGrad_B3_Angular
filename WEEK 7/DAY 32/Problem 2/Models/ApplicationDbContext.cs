using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }    //  Represent database table 

    }
}
