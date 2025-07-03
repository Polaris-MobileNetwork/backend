using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.TestResults
{
    public class SaveTestResultCommand : IRequest<SaveTestResultResult>
    {
        public long Timestamp { get; set; }
        public string TestType { get; set; } = string.Empty;
        public string? TargetHost { get; set; }
        public string ResultValue { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string? Details { get; set; }
        public Guid? TestId { get; set; }
    }

    public class SaveTestResultResult : ResultModel
    {
        public Guid Id { get; set; }
    }

    public class SaveTestResultsCommand : IRequest<SaveTestResultsResult>
    {
        public List<SaveTestResultCommand> TestResults { get; set; } = new List<SaveTestResultCommand>();
    }

    public class SaveTestResultsResult : ResultModel
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();
    }

    public class SaveTestResultHandler : IRequestHandler<SaveTestResultCommand, SaveTestResultResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public SaveTestResultHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<SaveTestResultResult> Handle(SaveTestResultCommand request, CancellationToken cancellationToken)
        {
            var result = new SaveTestResultResult();

            //var currentUserId = identityService.GetCurrentUserId();
            //if (!currentUserId.HasValue)
            //{
            //    result.Code = 401;
            //    result.Message = "User not authenticated";
            //    return result;
            //}

            if (request.TestId.HasValue)
            {
                var test = await uow.Tests.GetById(request.TestId.Value);
                if (test == null)
                {
                    result.Code = 404;
                    result.Message = "Test not found";
                    return result;
                }
            }

            var testResult = new TestResult
            {
                Id = Guid.NewGuid(),
                Timestamp = request.Timestamp,
                TestType = request.TestType,
                TargetHost = request.TargetHost,
                ResultValue = request.ResultValue,
                IsSuccess = request.IsSuccess,
                Details = request.Details,
                TestId = request.TestId
            };

            await uow.TestResults.Add(testResult);
            await uow.SaveChangesAsync();

            result.Success = true;
            result.Code = 200;
            result.Id = testResult.Id;

            return result;
        }
    }

    public class SaveTestResultsHandler : IRequestHandler<SaveTestResultsCommand, SaveTestResultsResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public SaveTestResultsHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<SaveTestResultsResult> Handle(SaveTestResultsCommand request, CancellationToken cancellationToken)
        {
            var result = new SaveTestResultsResult { Ids = new List<Guid>() };

            //var currentUserId = identityService.GetCurrentUserId();
            //if (!currentUserId.HasValue)
            //{
            //    result.Code = 401;
            //    result.Message = "User not authenticated";
            //    return result;
            //}

            // Optionally, validate TestId existence for each result (skip for performance, or batch query if needed)
            var testResults = request.TestResults.Select(tr => new TestResult
            {
                Id = Guid.NewGuid(),
                Timestamp = tr.Timestamp,
                TestType = tr.TestType,
                TargetHost = tr.TargetHost,
                ResultValue = tr.ResultValue,
                IsSuccess = tr.IsSuccess,
                Details = tr.Details,
                TestId = tr.TestId
            }).ToList();

            await uow.TestResults.AddRange(testResults);
            await uow.SaveChangesAsync();

            result.Success = true;
            result.Code = 200;
            result.Ids = testResults.Select(tr => tr.Id).ToList();

            return result;
        }
    }
} 