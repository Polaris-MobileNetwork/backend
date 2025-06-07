using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NetworkMeasurementRepository : INetworkMeasurementRepository
    {
        private readonly ApplicationDataContext dataContext;

        public NetworkMeasurementRepository(ApplicationDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task AddAsync(NetworkMeasurement measurement)
        {
            await dataContext.NetworkMeasurements.AddAsync(measurement);
        }

        public async Task<NetworkMeasurement> GetById(Guid id)
        {
            return await dataContext.NetworkMeasurements.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<NetworkMeasurement>> GetByUserId(Guid userId)
        {
            return await dataContext.NetworkMeasurements
                .Where(n => n.Id == userId)
                .ToListAsync();
        }
    }
} 