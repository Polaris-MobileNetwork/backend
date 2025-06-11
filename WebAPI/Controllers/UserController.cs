using Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {

        }


        [AllowAnonymous]
        [HttpPost(nameof(SignUp))]
        public async Task<ActionResult<SignUpResult>> SignUp([FromBody] SignUpCommand request)
        {
            return await mediator.Send(request);
        }


        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<ActionResult<LoginResult>> Login([FromBody]LoginCommand request)
        {
            return await mediator.Send(request);
        }
    }
}
