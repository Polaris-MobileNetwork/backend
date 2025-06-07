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

        [HttpGet("{id}")]
        public async Task<ActionResult<GetNetworkMeasurementResult>> Get(Guid id)
        {
            return await mediator.Send(new GetNetworkMeasurementCommand { Id = id });
        }
    }
} 