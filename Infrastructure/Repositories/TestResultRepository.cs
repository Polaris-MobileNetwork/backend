using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TestResultRepository : ITestResultRepository
    {
        private readonly ApplicationDataContext dataContext;

        public TestResultRepository(ApplicationDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<TestResult> GetById(Guid id)
        {
            return await dataContext.TestResults
                .Include(tr => tr.Test)
                .FirstOrDefaultAsync(tr => tr.Id == id);
        }

        public async Task<IEnumerable<TestResult>> GetByIds(IEnumerable<Guid> ids)
        {
            return await dataContext.TestResults
                .Include(tr => tr.Test)
                .Where(tr => ids.Contains(tr.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<TestResult>> GetByTestId(Guid testId, long? startTime = null, long? endTime = null)
        {
            var query = dataContext.TestResults
                .Include(tr => tr.Test)
                .Where(tr => tr.TestId == testId);

            if (startTime.HasValue)
            {
                query = query.Where(tr => tr.Timestamp >= startTime.Value);
            }

            if (endTime.HasValue)
            {
                query = query.Where(tr => tr.Timestamp <= endTime.Value);
            }

            return await query.ToListAsync();
        }

        public async Task Add(TestResult testResult)
        {
            await dataContext.TestResults.AddAsync(testResult);
        }

        public async Task AddRange(IEnumerable<TestResult> testResults)
        {
            await dataContext.TestResults.AddRangeAsync(testResults);
        }
    }
} 