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

        public async Task<(IEnumerable<NetworkMeasurement> Measurements, int TotalCount)> GetLatestMeasurements(int pageSize, int pageNumber)
        {
            var query = dataContext.NetworkMeasurements
                .OrderByDescending(n => n.TimeStamp);

            var totalCount = await query.CountAsync();
            var measurements = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (measurements, totalCount);
        }

        public async Task<IEnumerable<NetworkMeasurement>> GetMeasurementsInArea(double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            // Get all measurements in the area
            var measurements = await dataContext.NetworkMeasurements
                .Where(n => n.Latitude >= minLatitude && n.Latitude <= maxLatitude &&
                           n.Longitude >= minLongitude && n.Longitude <= maxLongitude)
                .OrderByDescending(n => n.TimeStamp)
                .ToListAsync();

            // Group by location (using rounded coordinates to group nearby points)
            // Round to 6 decimal places (approximately 11cm precision)
            var groupedMeasurements = measurements
                .GroupBy(n => new
                {
                    Latitude = Math.Round(n.Latitude ?? 0, 6),
                    Longitude = Math.Round(n.Longitude ?? 0, 6)
                })
                .Select(g => g.First()) // Take the most recent measurement for each location
                .ToList();

            return groupedMeasurements;
        }
    }
} 