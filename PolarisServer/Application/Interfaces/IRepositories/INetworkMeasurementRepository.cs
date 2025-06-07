using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface INetworkMeasurementRepository
    {
        Task<NetworkMeasurement> GetById(Guid id);
        Task AddAsync(NetworkMeasurement measurement);
        Task<IEnumerable<NetworkMeasurement>> GetByUserId(Guid userId);
    }
} 