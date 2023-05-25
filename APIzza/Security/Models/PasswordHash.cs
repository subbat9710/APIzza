namespace APIzza.Security.Models
{
    public class PasswordHash
    {
        public PasswordHash(string password, string salt)
        {
            this.Password = password;
            this.Salt = salt;
        }

        public string Password { get; }

        public string Salt { get; }
    }
}
