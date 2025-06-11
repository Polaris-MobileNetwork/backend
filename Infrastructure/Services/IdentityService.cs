using System.Security.Claims;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public IdentityService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public Guid? GetCurrentUserId()
        {
            string? userId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userId, out Guid result))
            {
                return result;
            }
            return null;
        }
    }
}
