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

    public class GetNetworkMeasurementsCommand : IRequest<GetNetworkMeasurementsResult>
    {
        public List<Guid> Ids { get; set; }
    }

    public class GetNetworkMeasurementsResult : ResultModel
    {
        public List<NetworkMeasurement> Measurements { get; set; }
    }

    public class GetLatestNetworkMeasurementsCommand : IRequest<GetLatestNetworkMeasurementsResult>
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }

    public class GetLatestNetworkMeasurementsResult : ResultModel
    {
        public List<NetworkMeasurement> Measurements { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetMeasurementsInAreaCommand : IRequest<GetMeasurementsInAreaResult>
    {
        public double MinLatitude { get; set; }
        public double MaxLatitude { get; set; }
        public double MinLongitude { get; set; }
        public double MaxLongitude { get; set; }
    }

    public class GetMeasurementsInAreaResult : ResultModel
    {
        public List<NetworkMeasurement> Measurements { get; set; }
    }

    public class GetMeasurementsByLocationAndTimeRangeCommand : IRequest<GetMeasurementsByLocationAndTimeRangeResult>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public double RadiusInMeters { get; set; } = 100;
    }

    public class GetMeasurementsByLocationAndTimeRangeResult : ResultModel
    {
        public List<NetworkMeasurement> Measurements { get; set; } = new();
    }

    public class GetMeasurementsByTimeRangeCommand : IRequest<GetMeasurementsByTimeRangeResult>
    {
        public long StartTime { get; set; }
        public long EndTime { get; set; }
    }

    public class GetMeasurementsByTimeRangeResult : ResultModel
    {
        public List<NetworkMeasurement> Measurements { get; set; } = new();
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

    public class GetNetworkMeasurementsHandler : IRequestHandler<GetNetworkMeasurementsCommand, GetNetworkMeasurementsResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public GetNetworkMeasurementsHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<GetNetworkMeasurementsResult> Handle(GetNetworkMeasurementsCommand request, CancellationToken cancellationToken)
        {
            var result = new GetNetworkMeasurementsResult { Measurements = new List<NetworkMeasurement>() };

            var currentUserId = identityService.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                result.Code = 401;
                result.Message = "User not authenticated";
                return result;
            }

            var measurements = await uow.NetworkMeasurements.GetByIds(request.Ids);
            if (!measurements.Any())
            {
                result.Code = 404;
                result.Message = "No network measurements found";
                return result;
            }

            result.Success = true;
            result.Code = 200;
            result.Measurements = measurements.ToList();

            return result;
        }
    }

    public class GetLatestNetworkMeasurementsHandler : IRequestHandler<GetLatestNetworkMeasurementsCommand, GetLatestNetworkMeasurementsResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public GetLatestNetworkMeasurementsHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<GetLatestNetworkMeasurementsResult> Handle(GetLatestNetworkMeasurementsCommand request, CancellationToken cancellationToken)
        {
            var result = new GetLatestNetworkMeasurementsResult
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber
            };

            var currentUserId = identityService.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                result.Code = 401;
                result.Message = "User not authenticated";
                return result;
            }

            var (measurements, totalCount) = await uow.NetworkMeasurements.GetLatestMeasurements(request.PageSize, request.PageNumber);
            
            result.Success = true;
            result.Code = 200;
            result.Measurements = measurements.ToList();
            result.TotalCount = totalCount;
            result.TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return result;
        }
    }

    public class GetMeasurementsInAreaHandler : IRequestHandler<GetMeasurementsInAreaCommand, GetMeasurementsInAreaResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public GetMeasurementsInAreaHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<GetMeasurementsInAreaResult> Handle(GetMeasurementsInAreaCommand request, CancellationToken cancellationToken)
        {
            var result = new GetMeasurementsInAreaResult { Measurements = new List<NetworkMeasurement>() };

            var currentUserId = identityService.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                result.Code = 401;
                result.Message = "User not authenticated";
                return result;
            }

            // Validate coordinates
            if (request.MinLatitude > request.MaxLatitude || request.MinLongitude > request.MaxLongitude)
            {
                result.Code = 400;
                result.Message = "Invalid coordinate range";
                return result;
            }

            var measurements = await uow.NetworkMeasurements.GetMeasurementsInArea(
                request.MinLatitude,
                request.MaxLatitude,
                request.MinLongitude,
                request.MaxLongitude
            );

            result.Success = true;
            result.Code = 200;
            result.Measurements = measurements.ToList();

            return result;
        }
    }

    public class GetMeasurementsByLocationAndTimeRangeHandler : IRequestHandler<GetMeasurementsByLocationAndTimeRangeCommand, GetMeasurementsByLocationAndTimeRangeResult>
    {
        private readonly IUnitOfWork uow;

        public GetMeasurementsByLocationAndTimeRangeHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<GetMeasurementsByLocationAndTimeRangeResult> Handle(GetMeasurementsByLocationAndTimeRangeCommand request, CancellationToken cancellationToken)
        {
            var result = new GetMeasurementsByLocationAndTimeRangeResult();

            var measurements = await uow.NetworkMeasurements.GetMeasurementsByLocationAndTimeRange(
                request.Latitude,
                request.Longitude,
                request.StartTime,
                request.EndTime,
                request.RadiusInMeters
            );

            result.Success = true;
            result.Code = 200;
            result.Measurements = measurements.ToList();

            return result;
        }
    }

    public class GetMeasurementsByTimeRangeHandler : IRequestHandler<GetMeasurementsByTimeRangeCommand, GetMeasurementsByTimeRangeResult>
    {
        private readonly IUnitOfWork uow;

        public GetMeasurementsByTimeRangeHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<GetMeasurementsByTimeRangeResult> Handle(GetMeasurementsByTimeRangeCommand request, CancellationToken cancellationToken)
        {
            var result = new GetMeasurementsByTimeRangeResult();

            var measurements = await uow.NetworkMeasurements.GetMeasurementsByTimeRange(
                request.StartTime,
                request.EndTime
            );

            result.Success = true;
            result.Code = 200;
            result.Measurements = measurements.ToList();

            return result;
        }
    }
} 