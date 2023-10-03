using API.Context;
using API.Models;
using API.Repository.Interface;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyContext context;
        public AccountRepository(MyContext context)
        {
            this.context = context;
        }
        public bool Login(string Email, string Password)
        {
            var email = context.Employees.FirstOrDefault(Employee => Employee.Email == Email);
            var password = context.Accounts.AsNoTracking().Single(account => account.NIK == email.NIK);

            bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(Password, password.password);
            return isValid;            
        }
        public Task SendEmailAsync(string email, string subject, string body)
        {
            var mail = "galaxiaquatope@outlook.com";
            var pw = "fayyad12345";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };
            return client.SendMailAsync(
                new MailMessage(from: mail,
                to: email,
                subject, body));
        }

    }
}
