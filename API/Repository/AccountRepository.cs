using API.Context;
using API.Models;
using API.Repository.Interface;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Win32;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyContext context;
        public IConfiguration config;
        public AccountRepository(MyContext context, IConfiguration configuration)
        {
            this.context = context;
            this.config = configuration;
        }
     
        public bool Login(string Email, string Password)
        {
            var email = context.Employees.FirstOrDefault(Employee => Employee.Email == Email);
            if (email == null)
            {
                return false;
            }
            var password = context.Accounts.AsNoTracking().Single(account => account.NIK == email.NIK);
            if (password == null)
            {
                return false;
            }
            bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(Password, password.password);
            return isValid;            
        }
        public Task SendEmailAsync(string email, string subject, string body)
        {
            var mail = config["EmailSettings:SmtpEmail"];
            var pw = config["EmailSettings:SmtpPassword"];

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
                    otp = resetToken,
                    otp_expire = DateTime.Now.AddMinutes(3),
                    isUsed = false
                };

                context.Entry(updateData).State = EntityState.Modified;
                context.SaveChanges();

             /*   var account = context.Accounts.Find(data.NIK);
                account.otp = resetToken;
                account.otp_expire = DateTime.Now.AddMinutes(3);
                account.isUsed = false;
                context.SaveChanges();*/

                SendEmailAsync(data.Email, subject, body);
                return true;
            }
        }
        public int ChangePassword(ChangePasswordVM ChangePassword)
        {
            var data = context.Employees.FirstOrDefault(employe => employe.Email == ChangePassword.Email);
            DateTime Now = DateTime.Now;
            if (data == null) 
            {
                return -1;
            } else
            {
                var account = context.Accounts.FirstOrDefault(account => account.NIK == data.NIK);
                var validasi = ValidatePassoword(ChangePassword.Password);
                bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(ChangePassword.Password, account.password);
                if (ChangePassword.Password != ChangePassword.PasswordConfirmed)
                {
                    return -2;
                } else if (ChangePassword.OTP != account.otp)
                {
                    return -3;
                } else if (DateTime.Now > account.otp_expire)
                {
                    return -4;
                } else if (account.isUsed == true)
                {
                    return -5;
                } else if (isValid)
                {
                    return -6;
                } else if (!validasi)
                {
                    return -7;
                } else
                {
                    string hashPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(ChangePassword.Password, 12);
                    account.password = hashPassword;
                    account.isUsed = true;
                    context.SaveChanges();
                    return 1;
                }
                
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
        public static bool ValidatePassoword(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            return true;
        }

    }
}
