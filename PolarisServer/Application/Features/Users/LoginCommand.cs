using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using MediatR;

namespace Application.Features.Users
{
    public class LoginCommand : IRequest<LoginResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResult : ResultModel
    {
        public string Token { get; set; }
    }

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IJwtService jwtService;
        private readonly IIdentityService identityService;

        public LoginHandler(IUnitOfWork uow, IJwtService jwtService, IIdentityService identityService)
        {
            this.uow = uow;
            this.jwtService = jwtService;
            this.identityService = identityService;
        }
        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = new LoginResult();

            var user = await uow.Users.GetUser(request.Username);
            if (user == null) {
                result.Code = 404;
                result.Message = "wrong username or password";
                return result;
            }

            var isCorrectPassword = identityService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);

            if (!isCorrectPassword)
            {
                result.Code = 401;
                result.Message = "wrong username or password";
                return result;
            }

            result.Token = jwtService.GenerateToken(user);

            result.Success= true;
            result.Code = 200;

            return result;
        }
    }

}
