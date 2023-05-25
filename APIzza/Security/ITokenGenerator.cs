namespace APIzza.Security
{
    public interface ITokenGenerator
    {
        string GenerateToken(int userId, string username);

        string GenerateToken(int userId, string username, string role);
    }
}
