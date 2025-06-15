using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly ApplicationDataContext dataContext;

        public TestRepository(ApplicationDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Test> GetById(Guid id)
        {
            return await dataContext.Tests.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Test>> GetByIds(IEnumerable<Guid> ids)
        {
            return await dataContext.Tests.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public async Task Add(Test test)
        {
            await dataContext.Tests.AddAsync(test);
        }

        public async Task AddRange(IEnumerable<Test> tests)
        {
            await dataContext.Tests.AddRangeAsync(tests);
        }

        public async Task Delete(Test test)
        {
            dataContext.Tests.Remove(test);
            await Task.CompletedTask;
        }
    }
} 