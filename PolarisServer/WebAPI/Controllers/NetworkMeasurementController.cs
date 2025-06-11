using Application.Features.NetworkMeasurements;
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

        [HttpPost(nameof(Save))]
        public async Task<ActionResult<SaveNetworkMeasurementResult>> Save([FromBody] SaveNetworkMeasurementCommand request)
        {
            return await mediator.Send(request);
        }

        [HttpPost(nameof(SaveMultiple))]
        public async Task<ActionResult<SaveNetworkMeasurementsResult>> SaveMultiple([FromBody] SaveNetworkMeasurementsCommand request)
        {
            return await mediator.Send(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetNetworkMeasurementResult>> Get(Guid id)
        {
            return await mediator.Send(new GetNetworkMeasurementCommand { Id = id });
        }

        [HttpPost(nameof(GetMultiple))]
        public async Task<ActionResult<GetNetworkMeasurementsResult>> GetMultiple([FromBody] GetNetworkMeasurementsCommand request)
        {
            return await mediator.Send(request);
        }
    }
} 