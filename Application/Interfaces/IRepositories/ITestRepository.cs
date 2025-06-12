using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface ITestRepository
    {
        Task<Test> GetById(Guid id);
        Task<IEnumerable<Test>> GetByIds(IEnumerable<Guid> ids);
        Task Add(Test test);
        Task AddRange(IEnumerable<Test> tests);
    }
} 