using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDataContext dataContext;

        public UserRepository(ApplicationDataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task AddAsync(User user)
        {
            await dataContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistsAsync(string username)
        {
            return await dataContext.Users.AnyAsync(u=> u.Username == username);
        }

        public async Task<User?> GetUser(Guid id)
        {
            return await dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUser(string username)
        {
            return await dataContext.Users.FirstOrDefaultAsync(u=> u.Username == username);
        }
    }
}
