using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; }
        public INetworkMeasurementRepository NetworkMeasurements { get; }
        Task<int> SaveChangesAsync();
    }
} 