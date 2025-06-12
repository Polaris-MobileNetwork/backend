using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface ITestResultRepository
    {
        Task<TestResult> GetById(Guid id);
        Task<IEnumerable<TestResult>> GetByIds(IEnumerable<Guid> ids);
        Task<IEnumerable<TestResult>> GetByTestId(Guid testId, long? startTime = null, long? endTime = null);
        Task Add(TestResult testResult);
        Task AddRange(IEnumerable<TestResult> testResults);
    }
} 