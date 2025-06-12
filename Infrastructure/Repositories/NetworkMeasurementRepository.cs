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
                .Select(g => g.First())
                .ToList();

            return groupedMeasurements;
        }

        public async Task<IEnumerable<NetworkMeasurement>> GetMeasurementsByLocationAndTimeRange(double latitude, double longitude, long startTime, long endTime, double radiusInMeters = 100)
        {
            // Convert radius from meters to degrees (approximate)
            // 1 degree of latitude is approximately 111km at the equator
            // 1 degree of longitude varies with latitude, but we'll use a rough approximation
            double latRadius = radiusInMeters / 111000.0;
            double lonRadius = radiusInMeters / (111000.0 * Math.Cos(latitude * Math.PI / 180.0));

            return await dataContext.NetworkMeasurements
                .Where(n => n.Latitude >= latitude - latRadius && 
                           n.Latitude <= latitude + latRadius &&
                           n.Longitude >= longitude - lonRadius && 
                           n.Longitude <= longitude + lonRadius &&
                           n.TimeStamp >= startTime && 
                           n.TimeStamp <= endTime)
                .OrderByDescending(n => n.TimeStamp)
                .ToListAsync();
        }
    }
} 