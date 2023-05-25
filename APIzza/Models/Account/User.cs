using System.Text.Json.Serialization;

namespace APIzza.Models.Account
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public string Role { get; set; }
    }
}
