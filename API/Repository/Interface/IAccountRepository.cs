using API.Models;
using API.ViewModel;

namespace API.Repository.Interface
{
    public interface IAccountRepository
    {
        bool Login(string Email, string Password);
        Task SendEmailAsync(string email, string subject, string body);
        bool ForgetPassword(string email);
        int ChangePassword(ChangePasswordVM ChangePassword);
    }
}
