using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<NetworkMeasurement> NetworkMeasurements { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<DeletedTest> DeletedTests { get; set; }
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext>  options) : base(options)
        {
            
        }
    }
}
