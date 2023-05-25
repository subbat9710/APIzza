using APIzza.Security.Models;

namespace APIzza.Security
{
    public interface IPasswordHasher
    {
        PasswordHash ComputeHash(string plainTextPassword);

        bool VerifyHashMatch(string existingHashedPassword, string plainTextPassword, string salt);
    }
}
