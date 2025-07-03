using Application.Interfaces.IRepositories;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Tests
{
    public class GetLatestTestsCommand : IRequest<GetLatestTestsResult>
    {
        public int Count { get; set; } = 100;
    }

    public class GetLatestTestsResult
    {
        public List<Test> Tests { get; set; } = new List<Test>();
    }

    public class GetLatestTestsHandler : IRequestHandler<GetLatestTestsCommand, GetLatestTestsResult>
    {
        private readonly ITestRepository testRepository;

        public GetLatestTestsHandler(ITestRepository testRepository)
        {
            this.testRepository = testRepository;
        }

        public async Task<GetLatestTestsResult> Handle(GetLatestTestsCommand request, CancellationToken cancellationToken)
        {
            var tests = await testRepository.GetLatestTests(request.Count);
            return new GetLatestTestsResult { Tests = tests.ToList() };
        }
    }
}