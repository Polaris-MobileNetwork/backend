using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests
{
    public class SaveTestCommand : IRequest<SaveTestResult>
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ParametersJson { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public long? ScheduledTimestamp { get; set; }
        public int? IntervalSeconds { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class SaveTestResult : ResultModel
    {
        public Guid Id { get; set; }
    }

    public class SaveTestHandler : IRequestHandler<SaveTestCommand, SaveTestResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public SaveTestHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<SaveTestResult> Handle(SaveTestCommand request, CancellationToken cancellationToken)
        {
            var result = new SaveTestResult();

            //var currentUserId = identityService.GetCurrentUserId();
            //if (!currentUserId.HasValue)
            //{
            //    result.Code = 401;
            //    result.Message = "User not authenticated";
            //    return result;
            //}

            var test = new Test
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Type = request.Type,
                ParametersJson = request.ParametersJson,
                IsEnabled = request.IsEnabled,
                ScheduledTimestamp = request.ScheduledTimestamp,
                IntervalSeconds = request.IntervalSeconds,
                IsCompleted = request.IsCompleted,
            };

            await uow.Tests.Add(test);
            await uow.SaveChangesAsync();

            result.Success = true;
            result.Code = 200;
            result.Id = test.Id;

            return result;
        }
    }
} 