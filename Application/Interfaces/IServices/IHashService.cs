namespace Application.Interfaces.IServices
{
    public interface IHashService
    {
        (byte[] passwordHash, byte[] passwordSalt) HashPassword(string password);
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
