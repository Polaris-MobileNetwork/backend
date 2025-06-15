using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface IDeletedTestRepository
    {
        Task Add(DeletedTest deletedTest);
        Task<IEnumerable<DeletedTest>> GetAll();
    }
} 