using Domain.Entities;

namespace Application.Interfaces.IServices
{
    public interface IJwtService
    {
        public string GenerateToken(User user);
    }
}
