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

        public async Task AddRangeAsync(IEnumerable<NetworkMeasurement> measurements)
        {
            await dataContext.NetworkMeasurements.AddRangeAsync(measurements);
        }

        public async Task<NetworkMeasurement> GetById(Guid id)
        {
            return await dataContext.NetworkMeasurements.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<NetworkMeasurement>> GetByIds(IEnumerable<Guid> ids)
        {
            return await dataContext.NetworkMeasurements
                .Where(n => ids.Contains(n.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<NetworkMeasurement>> GetByUserId(Guid userId)
        {
            return await dataContext.NetworkMeasurements
                .Where(n => n.Id == userId)
                .ToListAsync();
        }
    }
} 