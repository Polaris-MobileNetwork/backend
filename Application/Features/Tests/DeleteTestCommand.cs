using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests
{
    public class DeleteTestCommand : IRequest<DeleteTestResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteTestResult : ResultModel
    {
        public Guid DeletedTestId { get; set; }
    }

    public class DeleteTestHandler : IRequestHandler<DeleteTestCommand, DeleteTestResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public DeleteTestHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<DeleteTestResult> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
        {
            var result = new DeleteTestResult();

            //var currentUserId = identityService.GetCurrentUserId();
            //if (!currentUserId.HasValue)
            //{
            //    result.Code = 401;
            //    result.Message = "User not authenticated";
            //    return result;
            //}

            var test = await uow.Tests.GetById(request.Id);
            if (test == null)
            {
                result.Code = 404;
                result.Message = "Test not found";
                return result;
            }

            var deletedTest = new DeletedTest
            {
                Id = Guid.NewGuid(),
                DeletedTestId = test.Id
            };

            await uow.DeletedTests.Add(deletedTest);
            await uow.Tests.Delete(test);
            await uow.SaveChangesAsync();

            result.Success = true;
            result.Code = 200;
            result.DeletedTestId = test.Id;

            return result;
        }
    }
} 