using API.Context;
using API.Controllers;
using API.Models;
using API.Repository.Interface;
using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Net;
using System.Security;

namespace API.Repository
{
  
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext context;
        

        public EmployeeRepository(MyContext context)
        {
            this.context = context;
        }
        public int Register(RegisterVM register)
        {
            var employee = new Employee
            {
                NIK = CustomNIK(),
                FirstName = register.FirstName,
                LastName = register.LastName,
                Phone = register.Phone,
                BirthDate = register.BirthDate,
                Salary = register.Salary,
                Email = register.Email,
                Gender = register.Gender,
            };
            context.Employees.Add(employee);
            var setEmployee = context.SaveChanges();

            var account = new Account
            {
                NIK = employee.NIK,
                password = register.Password,
            };
            context.Accounts.Add(account);
            var setAccount = context.SaveChanges();

            var education = new Education
            {
                Degree = register.Degree,
                GPA = register.GPA,
                University_Id = register.University_Id,
            };
            context.Educations.Add(education);
            var setEducation = context.SaveChanges();

            var profilling = new Profilling
            {
                NIK = employee.NIK,
                Education_id = education.Id
            };
            context.Profillings.Add(profilling);
            var setProfilling = context.SaveChanges();
            if ((setEmployee > 0) && (setAccount > 0) && (setEducation > 0) && (setProfilling > 0))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }

        public Employee Get(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            return entity;
        }
        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            context.Remove(entity);
            var result = context.SaveChanges();
            return result;
        }

        public int Insert(Employee employee)
        {
           
            
            employee.NIK = CustomNIK();
            context.Employees.Add(employee);
            var result = context.SaveChanges();
            return result;
        }

        private string CustomNIK()
        {
            string date = DateTime.Now.ToString("ddMMyy");
            string newNIK = "";

            var lastData = context.Employees.OrderBy(employee => employee.NIK).LastOrDefault();
            if (lastData == null)
            {
                newNIK = date + "001";
            }
            else
            {
                var nikLastData = lastData.NIK;
                string lastThree = nikLastData.Substring(nikLastData.Length - 3);


                int nextSequence = int.Parse(lastThree) + 1;
                newNIK = date + nextSequence.ToString("000");
                /*return current date tostring ("D3")*/
            }
            return newNIK;
        }

        public int Update(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
           
            return result;
           /* throw new NotImplementedException();*/
        }

        
        public bool CheckEmail(Employee employee)
        {
             return context.Employees.Any(e => e.Email == employee.Email);
            /*if (context.Employees.Any(e => e.Email == employee.Email))
            {
                return false;
            }
            else return true;*/
        }
        public bool CheckPhoneNumber(Employee employee)
        {
            return context.Employees.Any(e => e.Phone == employee.Phone);
        }
    }
}
