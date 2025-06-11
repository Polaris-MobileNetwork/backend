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
    }
} 