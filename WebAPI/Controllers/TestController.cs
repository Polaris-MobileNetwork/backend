using Application.Features.Tests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class TestController : BaseController
    {
        public TestController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost(nameof(Save))]
        public async Task<ActionResult<SaveTestResult>> Save([FromBody] SaveTestCommand request)
        {
            return await mediator.Send(request);
        }

        [AllowAnonymous]
        [HttpPost(nameof(GetMultiple))]
        public async Task<ActionResult<GetTestsResult>> GetMultiple([FromBody] GetTestsCommand request)
        {
            return await mediator.Send(request);
        }
    }
} 