using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDataContext : DbContext
    {

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext>  options) : base(options)
        {
            
        }
    }
}
