using API.Context;
using API.Models;
using API.Repository.Interface;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;

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

    }
}
