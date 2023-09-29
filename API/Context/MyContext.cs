using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        /*Nama Tabel Diambil dari Employees, kalau mau dari model, pakai [Table("NamaTable")]*/
    }

}
