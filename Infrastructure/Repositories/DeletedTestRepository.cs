using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class DeletedTestRepository : IDeletedTestRepository
    {
        private readonly ApplicationDataContext dataContext;

        public DeletedTestRepository(ApplicationDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Add(DeletedTest deletedTest)
        {
            await dataContext.DeletedTests.AddAsync(deletedTest);
        }
    }
} 