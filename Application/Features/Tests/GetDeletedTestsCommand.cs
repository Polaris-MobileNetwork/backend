using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests
{
    public class GetDeletedTestsCommand : IRequest<GetDeletedTestsResult>
    {
    }

    public class GetDeletedTestsResult : ResultModel
    {
        public List<Guid> DeletedTestIds { get; set; } = new List<Guid>();
    }

    public class GetDeletedTestsHandler : IRequestHandler<GetDeletedTestsCommand, GetDeletedTestsResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public GetDeletedTestsHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<GetDeletedTestsResult> Handle(GetDeletedTestsCommand request, CancellationToken cancellationToken)
        {
            var result = new GetDeletedTestsResult();

            //var currentUserId = identityService.GetCurrentUserId();
            //if (!currentUserId.HasValue)
            //{
            //    result.Code = 401;
            //    result.Message = "User not authenticated";
            //    return result;
            //}

            var deletedTests = await uow.DeletedTests.GetAll();
            if (!deletedTests.Any())
            {
                result.Code = 404;
                result.Message = "No deleted tests found";
                return result;
            }

            result.Success = true;
            result.Code = 200;
            result.DeletedTestIds = deletedTests.Select(dt => dt.DeletedTestId).ToList();

            return result;
        }
    }
} 