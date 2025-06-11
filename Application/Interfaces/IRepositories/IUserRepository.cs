using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUser(Guid id);
        Task<User?> GetUser(string username);
        Task AddAsync(User user);
        Task<bool> ExistsAsync(string username);
    }
}
