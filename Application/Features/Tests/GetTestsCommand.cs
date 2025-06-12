using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests
{
    public class GetTestsCommand : IRequest<GetTestsResult>
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();
    }

    public class GetTestsResult : ResultModel
    {
        public List<Test> Tests { get; set; } = new List<Test>();
    }

    public class GetTestsHandler : IRequestHandler<GetTestsCommand, GetTestsResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public GetTestsHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<GetTestsResult> Handle(GetTestsCommand request, CancellationToken cancellationToken)
        {
            var result = new GetTestsResult();

            var currentUserId = identityService.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                result.Code = 401;
                result.Message = "User not authenticated";
                return result;
            }

            var tests = await uow.Tests.GetByIds(request.Ids);
            if (!tests.Any())
            {
                result.Code = 404;
                result.Message = "No tests found";
                return result;
            }

            result.Success = true;
            result.Code = 200;
            result.Tests = tests.ToList();

            return result;
        }
    }
} 