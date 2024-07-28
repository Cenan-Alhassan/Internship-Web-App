using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
       public DbSet<DepartmentEntity> Departments {get; set;}
       public DbSet<EmployeeEntity> Employees {get; set;}

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
        modelBuilder
            .Entity<EmployeeEntity>()
            .Property(e => e.Type)
            .HasConversion(
                v => v.ToString(),
                v => (EmployeeLevel)Enum.Parse(typeof(EmployeeLevel), v));
       }

    }
}