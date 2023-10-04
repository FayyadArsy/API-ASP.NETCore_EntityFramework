using API.Context;
using API.Models;
using API.Repository.Interface;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Win32;

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

        public bool ForgetPassword(string email)
        {
            var data = context.Employees.Join(context.Accounts, emp => emp.NIK,
                                                                acc => acc.NIK,
                                                                (emp, acc) => new
                                                                {
                                                                    NIK = emp.NIK,
                                                                    FullName = emp.FirstName + " " + emp.LastName,
                                                                    Email = emp.Email,
                                                                    Password = acc.password,
                                                                    OTP = acc.otp,
                                                                    Expired = acc.otp_expire

                                                                }).SingleOrDefault(e => e.Email == email);
            if (data == null)
            {
                return false;
            } else
            {
                string resetToken = GenerateOTP(); 

            
                string subject = "Password Reset";
                string body = $"Dear {data.FullName},\n\n";
                body += $"Kode ini bersifat rahasia: {resetToken}";

                var updateData = new Account
                {
                    NIK = data.NIK,
                    password = data.Password,
                    otp =  resetToken ,
                    otp_expire = DateTime.Now
                    
                };

                context.Entry(updateData).State = EntityState.Modified;
                var result = context.SaveChanges();
                SendEmailAsync(data.Email, subject, body);
                return true;
            }
        }
        public static string GenerateOTP()
        {
            const string characters = "0123456789"; 
            Random random = new Random();
            StringBuilder otp = new StringBuilder(6);

            for (int i = 0; i < 6; i++)
            {
                int index = random.Next(0, characters.Length);
                otp.Append(characters[index]);
            }

            return otp.ToString();
        }

    }
}
