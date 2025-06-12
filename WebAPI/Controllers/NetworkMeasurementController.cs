using Application.Features.NetworkMeasurements;
using Application.Features.NetworkMeasurements.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class NetworkMeasurementController : BaseController
    {
        public NetworkMeasurementController(IMediator mediator) : base(mediator)
        {
        }
        [AllowAnonymous]
        [HttpPost(nameof(Save))]
        public async Task<ActionResult<SaveNetworkMeasurementResult>> Save([FromBody] SaveNetworkMeasurementCommand request)
        {
            return await mediator.Send(request);
        }

        [AllowAnonymous]
        [HttpPost(nameof(SaveMultiple))]
        public async Task<ActionResult<SaveNetworkMeasurementsResult>> SaveMultiple([FromBody] SaveNetworkMeasurementsCommand request)
        {
            return await mediator.Send(request);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetNetworkMeasurementResult>> Get(Guid id)
        {
            return await mediator.Send(new GetNetworkMeasurementCommand { Id = id });
        }

        [AllowAnonymous]
        [HttpPost(nameof(GetMultiple))]
        public async Task<ActionResult<GetNetworkMeasurementsResult>> GetMultiple([FromBody] GetNetworkMeasurementsCommand request)
        {
            return await mediator.Send(request);
        }

        [AllowAnonymous]
        [HttpGet("latest")]
        public async Task<ActionResult<GetLatestNetworkMeasurementsResult>> GetLatest([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
        {
            return await mediator.Send(new GetLatestNetworkMeasurementsCommand 
            { 
                PageSize = pageSize,
                PageNumber = pageNumber
            });
        }

        [AllowAnonymous]
        [HttpGet("area")]
        public async Task<ActionResult<GetMeasurementsInAreaResult>> GetInArea([FromQuery] AreaParametersDto parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await mediator.Send(new GetMeasurementsInAreaCommand
            {
                MinLatitude = parameters.MinLatitude,
                MaxLatitude = parameters.MaxLatitude,
                MinLongitude = parameters.MinLongitude,
                MaxLongitude = parameters.MaxLongitude
            });
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetMeasurementsByLocationAndTimeRange(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] long startTime,
            [FromQuery] long endTime,
            [FromQuery] double radiusInMeters = 100)
        {
            if (startTime > endTime)
            {
                return BadRequest("Start time must be before end time");
            }

            if (startTime > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                return BadRequest("Start time cannot be in the future");
            }

            var command = new GetMeasurementsByLocationAndTimeRangeCommand
            {
                Latitude = latitude,
                Longitude = longitude,
                StartTime = startTime,
                EndTime = endTime,
                RadiusInMeters = radiusInMeters
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("time-range")]
        public async Task<IActionResult> GetMeasurementsByTimeRange(
            [FromQuery] long startTime,
            [FromQuery] long endTime)
        {
            if (startTime > endTime)
            {
                return BadRequest("Start time must be before end time");
            }

            if (startTime > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                return BadRequest("Start time cannot be in the future");
            }

            var command = new GetMeasurementsByTimeRangeCommand
            {
                StartTime = startTime,
                EndTime = endTime
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
} 