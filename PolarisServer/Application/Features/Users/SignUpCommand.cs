using System.Net;
using Application.Common.Models;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users
{
    public class SignUpCommand : IRequest<SignUpResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignUpResult : ResultModel
    {

    }

    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResult>
    {
        private readonly IUnitOfWork uow;
        private readonly IIdentityService identityService;

        public SignUpHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            this.uow = uow;
            this.identityService = identityService;
        }
        public async Task<SignUpResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var result = new SignUpResult();

            if (string.IsNullOrEmpty(request.Username) || request.Username.Length < 3) 
            {
                result.Code = 401;
                result.Message = "invalid username";
                return result;
            }

            if(await uow.Users.ExistsAsync(request.Username))
            {
                result.Code = 401;
                result.Message = "username exists";
                return result;
            }

            if(string.IsNullOrEmpty(request.Password) || request.Password.Length < 4)
            {
                result.Code = 401;
                result.Message = "invalid password";
                return result;
            }

            (byte[] passwordHash, byte[] passwordSalt) = identityService.HashPassword(request.Password);


            User user = new()
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await uow.Users.AddAsync(user);
            await uow.SaveChangesAsync();

            result.Success = true;
            result.Code = 200;
           
            return result;

        }
    }
}
