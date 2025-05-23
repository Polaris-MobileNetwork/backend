using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDataContext dataContext;

        public IUserRepository Users { get; }

        public UnitOfWork(ApplicationDataContext dataContext,  IUserRepository userRepository)
        {
            this.dataContext = dataContext;
            this.Users = userRepository;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await dataContext.SaveChangesAsync();
        }
    }
}
