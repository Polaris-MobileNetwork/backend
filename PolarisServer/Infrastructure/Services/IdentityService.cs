using System.Security.Cryptography;
using Application.Interfaces.IServices;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private const int hashSize = 32;
        private const int saltSize = 32;
        private const int maxIterations = 100000;
        
        public (byte[] passwordHash, byte[] passwordSalt) HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(saltSize);

            using var pbk = new Rfc2898DeriveBytes(password, salt, maxIterations, HashAlgorithmName.SHA256);
            var hash = pbk.GetBytes(hashSize);

            return (hash, salt);
        }

        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var pbk = new Rfc2898DeriveBytes(password, passwordSalt,maxIterations, HashAlgorithmName.SHA256);
            var computedHash = pbk.GetBytes(hashSize);

            return CryptographicOperations.FixedTimeEquals(passwordHash, computedHash);
        }
    }
}
