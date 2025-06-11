using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.NetworkMeasurements
{
    public class SaveNetworkMeasurementCommand : IRequest<SaveNetworkMeasurementResult>
    {
        public long TimeStamp { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string NetworkType { get; set; }
        public string? PLMNId { get; set; }
        public int? Lac { get; set; }
        public int? Tac { get; set; }
        public int? Rac { get; set; }
        public string? CellId { get; set; }
        public int? ARFCN { get; set; }
        public string? FrequencyBand { get; set; }
        public double? ActualFrequencyMhz { get; set; }
        public int SignalStrength { get; set; }
        public int? RSRP { get; set; }
        public int? RSRQ { get; set; }
        public int? RSCP { get; set; }
        public int? RXLEV { get; set; }
        public double? ECNO { get; set; }
    }

    public class SaveNetworkMeasurementResult : ResultModel
    {
        public Guid Id { get; set; }
    }

    public class SaveNetworkMeasurementsCommand : IRequest<SaveNetworkMeasurementsResult>
    {
        public List<SaveNetworkMeasurementCommand> Measurements { get; set; }
    }

    public class SaveNetworkMeasurementsResult : ResultModel
    {
        public List<Guid> Ids { get; set; }
    }

    public class SaveNetworkMeasurementHandler : IRequestHandler<SaveNetworkMeasurementCommand, SaveNetworkMeasurementResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public SaveNetworkMeasurementHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<SaveNetworkMeasurementResult> Handle(SaveNetworkMeasurementCommand request, CancellationToken cancellationToken)
        {
            var result = new SaveNetworkMeasurementResult();

            var currentUserId = identityService.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                result.Code = 401;
                result.Message = "User not authenticated";
                return result;
            }

            var measurement = new NetworkMeasurement
            {
                Id = Guid.NewGuid(),
                TimeStamp = request.TimeStamp,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                NetworkType = request.NetworkType,
                PLMNId = request.PLMNId,
                Lac = request.Lac,
                Tac = request.Tac,
                Rac = request.Rac,
                CellId = request.CellId,
                ARFCN = request.ARFCN,
                FrequencyBand = request.FrequencyBand,
                ActualFrequencyMhz = request.ActualFrequencyMhz,
                SignalStrength = request.SignalStrength,
                RSRP = request.RSRP,
                RSRQ = request.RSRQ,
                RSCP = request.RSCP,
                RXLEV = request.RXLEV,
                ECNO = request.ECNO
            };

            await uow.NetworkMeasurements.AddAsync(measurement);
            await uow.SaveChangesAsync();

            result.Success = true;
            result.Code = 200;
            result.Id = measurement.Id;

            return result;
        }
    }

    public class SaveNetworkMeasurementsHandler : IRequestHandler<SaveNetworkMeasurementsCommand, SaveNetworkMeasurementsResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public SaveNetworkMeasurementsHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }

        public async Task<SaveNetworkMeasurementsResult> Handle(SaveNetworkMeasurementsCommand request, CancellationToken cancellationToken)
        {
            var result = new SaveNetworkMeasurementsResult { Ids = new List<Guid>() };

            var currentUserId = identityService.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                result.Code = 401;
                result.Message = "User not authenticated";
                return result;
            }

            var measurements = request.Measurements.Select(m => new NetworkMeasurement
            {
                Id = Guid.NewGuid(),
                TimeStamp = m.TimeStamp,
                Latitude = m.Latitude,
                Longitude = m.Longitude,
                NetworkType = m.NetworkType,
                PLMNId = m.PLMNId,
                Lac = m.Lac,
                Tac = m.Tac,
                Rac = m.Rac,
                CellId = m.CellId,
                ARFCN = m.ARFCN,
                FrequencyBand = m.FrequencyBand,
                ActualFrequencyMhz = m.ActualFrequencyMhz,
                SignalStrength = m.SignalStrength,
                RSRP = m.RSRP,
                RSRQ = m.RSRQ,
                RSCP = m.RSCP,
                RXLEV = m.RXLEV,
                ECNO = m.ECNO
            }).ToList();

            await uow.NetworkMeasurements.AddRangeAsync(measurements);
            await uow.SaveChangesAsync();

            result.Success = true;
            result.Code = 200;
            result.Ids = measurements.Select(m => m.Id).ToList();

            return result;
        }
    }
} 