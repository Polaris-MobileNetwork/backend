using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext>  options) : base(options)
        {
            
        }
    }
}
