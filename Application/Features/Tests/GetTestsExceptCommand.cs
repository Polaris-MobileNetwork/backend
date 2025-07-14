using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests
{
    public class GetTestsExceptCommand : IRequest<GetTestsExceptResult>
    {
        public List<Guid> ExcludedIds { get; set; } = new List<Guid>();
    }

    public class GetTestsExceptResult : ResultModel
    {
        public List<Test> Tests { get; set; } = new List<Test>();
    }

    public class GetTestsExceptHandler : IRequestHandler<GetTestsExceptCommand, GetTestsExceptResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public GetTestsExceptHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<GetTestsExceptResult> Handle(GetTestsExceptCommand request, CancellationToken cancellationToken)
        {
            var result = new GetTestsExceptResult();

            //var currentUserId = identityService.GetCurrentUserId();
            //if (!currentUserId.HasValue)
            //{
            //    result.Code = 401;
            //    result.Message = "User not authenticated";
            //    return result;
            //}

            var tests = await uow.Tests.GetAllExcept(request.ExcludedIds);
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