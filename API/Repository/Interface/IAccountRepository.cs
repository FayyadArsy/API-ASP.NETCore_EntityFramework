using API.Models;

namespace API.Repository.Interface
{
    public interface IAccountRepository
    {
        bool Login(string Email, string Password);
        Task SendEmailAsync(string email, string subject, string body);
    }
}
