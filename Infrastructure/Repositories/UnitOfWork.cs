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
        public INetworkMeasurementRepository NetworkMeasurements { get; }

        public UnitOfWork(
            ApplicationDataContext dataContext,
            IUserRepository userRepository,
            INetworkMeasurementRepository networkMeasurementRepository)
        {
            this.dataContext = dataContext;
            this.Users = userRepository;
            this.NetworkMeasurements = networkMeasurementRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dataContext.SaveChangesAsync();
        }
    }
}
