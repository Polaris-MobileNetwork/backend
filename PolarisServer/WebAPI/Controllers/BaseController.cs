using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected readonly IMediator mediator;

        public BaseController(IMediator mediatorW)
        {
            this.mediator = mediator;
        }
    }

}
