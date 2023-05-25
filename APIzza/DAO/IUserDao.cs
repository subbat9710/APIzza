using APIzza.Models.Account;

namespace APIzza.DAO
{
    public interface IUserDao
    {
        User GetUser(string username);
        User AddUser(string username, string password, string role);
    }
}
