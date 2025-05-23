using Application.Common.Models;
using MediatR;

namespace Application.Features.Users
{
    public class LoginCommand : IRequest<LoginResult>
    {

    }
    public class LoginResult : ResultModel
    {

    }

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        public Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
