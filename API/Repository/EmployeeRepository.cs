using API.Context;
using API.Controllers;
using API.Models;
using API.Repository.Interface;
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
            employee.NIK = newNIK;
            context.Employees.Add(employee);
            var result = context.SaveChanges();
            return result;
        }

        public int Update(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
           
            return result;
           /* throw new NotImplementedException();*/
        }

        public List<string>  CustomNIK(Employee employee, int totalNIK)
        {
            var hasil = new List<string>();
            var tanggal = DateTime.Now.ToString("ddMMyyyy");
            for(int i=0; i < totalNIK; i++)
            {
                var custom = tanggal + i.ToString().PadLeft(2, '0');
                hasil.Add(custom);
            }
            return hasil;
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
