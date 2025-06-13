using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.TestResults
{
    public class GetTestResultsCommand : IRequest<GetTestResultsResult>
    {
        public Guid TestId { get; set; }
        public long? StartTime { get; set; }
        public long? EndTime { get; set; }
    }

    public class GetTestResultsResult : ResultModel
    {
        public List<TestResult> TestResults { get; set; } = new List<TestResult>();
    }

    public class GetTestResultsHandler : IRequestHandler<GetTestResultsCommand, GetTestResultsResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public GetTestResultsHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<GetTestResultsResult> Handle(GetTestResultsCommand request, CancellationToken cancellationToken)
        {
            var result = new GetTestResultsResult();

            var currentUserId = identityService.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                result.Code = 401;
                result.Message = "User not authenticated";
                return result;
            }

            var test = await uow.Tests.GetById(request.TestId);
            if (test == null)
            {
                result.Code = 404;
                result.Message = "Test not found";
                return result;
            }

            var testResults = await uow.TestResults.GetByTestId(
                request.TestId,
                request.StartTime,
                request.EndTime
            );

            if (!testResults.Any())
            {
                result.Code = 404;
                result.Message = "No test results found";
                return result;
            }

            result.Success = true;
            result.Code = 200;
            result.TestResults = testResults.ToList();

            return result;
        }
    }
} 