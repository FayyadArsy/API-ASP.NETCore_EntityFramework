using API.Models;

namespace API.Repository.Interface
{
    public interface IAccountRepository
    {
        bool Login(string Email, string Password);
    }
}
