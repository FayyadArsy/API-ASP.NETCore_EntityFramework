using API.Context;
using API.Controllers;
using API.Models;
using API.Repository.Interface;
using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Diagnostics.Metrics;
using System.Net;
using System.Numerics;
using System.Security;
using System.Threading.Tasks.Dataflow;

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
                password = BCrypt.Net.BCrypt.EnhancedHashPassword(register.Password, 12),
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
        public IEnumerable<GetVM> Getv2()
        {

            var query = (from employee in context.Employees
                         join profiling in context.Profillings on employee.NIK equals profiling.NIK
                         join education in context.Educations on profiling.Education_id equals education.Id
                         join university in context.Universitys on education.University_Id equals university.Id
                         select new GetVM
                         {
                             FullName = employee.FirstName + " " + employee.LastName,
                             Phone = employee.Phone,
                             BirthDate = employee.BirthDate,
                             Salary = employee.Salary,
                             Email = employee.Email,
                             Gender = employee.Gender,
                             Degree = education.Degree,
                             GPA = education.GPA,
                             University = university.Name,
                             


                         }).ToList();
                        


            return query;
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

        
        public bool CheckEmail(string email)
        {
             return context.Employees.Any(employee => employee.Email == email);
            /*if (context.Employees.Any(e => e.Email == employee.Email))
            {
                return false;
            }
            else return true;*/
        }
        public bool CheckPhoneNumber(string phone)
        {
            var data = context.Employees.AsNoTracking().FirstOrDefault(employee => employee.Phone == phone);
            if (data == null)
            {
                return false;
            }
            return true;
        }
   /*     public bool CheckUniversity(Employee employee)
        {
            var data = context.Universities.Find(id);
            if (data == null)
            {
                return false;
            }
            return true;
        }*/

       
    }
}
