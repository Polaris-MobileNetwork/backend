using Application.Features.TestResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class TestResultController : BaseController
    {
        public TestResultController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost(nameof(Save))]
        public async Task<ActionResult<SaveTestResultResult>> Save([FromBody] SaveTestResultCommand request)
        {
            return await mediator.Send(request);
        }

        [AllowAnonymous]
        [HttpGet("by-test/{testId}")]
        public async Task<ActionResult<GetTestResultsResult>> GetByTest(
            Guid testId,
            [FromQuery] long? startTime = null,
            [FromQuery] long? endTime = null)
        {
            return await mediator.Send(new GetTestResultsCommand
            {
                TestId = testId,
                StartTime = startTime,
                EndTime = endTime
            });
        }
    }
} 