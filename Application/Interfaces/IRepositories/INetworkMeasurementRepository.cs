using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface INetworkMeasurementRepository
    {
        Task<NetworkMeasurement> GetById(Guid id);
        Task<IEnumerable<NetworkMeasurement>> GetByIds(IEnumerable<Guid> ids);
        Task AddAsync(NetworkMeasurement measurement);
        Task AddRangeAsync(IEnumerable<NetworkMeasurement> measurements);
        Task<IEnumerable<NetworkMeasurement>> GetByUserId(Guid userId);
        Task<(IEnumerable<NetworkMeasurement> Measurements, int TotalCount)> GetLatestMeasurements(int pageSize, int pageNumber);
        Task<IEnumerable<NetworkMeasurement>> GetMeasurementsInArea(double minLatitude, double maxLatitude, double minLongitude, double maxLongitude);
    }
} 