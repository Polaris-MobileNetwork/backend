namespace Application.Interfaces.IServices
{
    public interface IIdentityService
    {
        (byte[] passwordHash, byte[] passwordSalt) HashPassword(string password);
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
