using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.NetworkMeasurements
{
    public class GetNetworkMeasurementCommand : IRequest<GetNetworkMeasurementResult>
    {
        public Guid Id { get; set; }
    }

    public class GetNetworkMeasurementResult : ResultModel
    {
        public NetworkMeasurement? Measurement { get; set; }
    }

    public class GetNetworkMeasurementHandler : IRequestHandler<GetNetworkMeasurementCommand, GetNetworkMeasurementResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public GetNetworkMeasurementHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<GetNetworkMeasurementResult> Handle(GetNetworkMeasurementCommand request, CancellationToken cancellationToken)
        {
            var result = new GetNetworkMeasurementResult();

            var currentUserId = identityService.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                result.Code = 401;
                result.Message = "User not authenticated";
                return result;
            }

            var measurement = await uow.NetworkMeasurements.GetById(request.Id);
            if (measurement == null)
            {
                result.Code = 404;
                result.Message = "Network measurement not found";
                return result;
            }

            result.Success = true;
            result.Code = 200;
            result.Measurement = measurement;

            return result;
        }
    }
} 