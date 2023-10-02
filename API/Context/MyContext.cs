using API.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Emit;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Profilling> Profillings { get; set; }
        public DbSet<University> Universitys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Education>()
                .HasMany(c => c.Profillings)
                .WithOne(e => e.Education);
            /*  .HasForeignKey(e => e.NIK);*/
            modelBuilder.Entity<Education>()
                .HasOne(e => e.University)
                .WithMany(c => c.Educations);
                base.OnModelCreating(modelBuilder);
        }


        /*Nama Tabel Diambil dari Employees, kalau mau dari model, pakai [Table("NamaTable")]*/
    }

}
