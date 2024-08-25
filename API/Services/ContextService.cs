using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class ContextService : IContextService
    {
        public void ConfigureNumOfEmployeesDepartmentsAndManagers(DataContext context)
{
    // Load all departments and employees in a single query
    var departments = context.Departments.ToList();
    var employees = context.Employees.Where(x => !x.IsDeleted).ToList();

    foreach (var department in departments)
    {
        var departmentName = department.Department;
        var numOfEmployees = employees.Count(e => e.Department == departmentName);
        var manager = employees
            .Where(e => e.Department == departmentName && e.Type == EmployeeLevel.Manager)
            .Select(e => e.Name)
            .FirstOrDefault() ?? "none";

        department.NumOfEmployee = numOfEmployees;
        department.Manager = manager;
    }

    foreach (var employee in employees)
    {
        if (!departments.Any(d => d.Department == employee.Department && !d.IsDeleted))
        {
            employee.Department = "none";
        }
    }
}

        public bool CheckIfManagerExists (DataContext context, EmployeeDto employeeDto, int Id)
        {

            foreach (var Employee in context.Employees.Where(x=> x.IsDeleted == false && x.Id != Id && x.Department == employeeDto.Department))
            {
                if (Employee.Type == EmployeeLevel.Manager) 
                {
                    return true;
                }
            }

            return false;
        }


        public void SetAsSoleManager (DataContext context, DepartmentDto departmentDto)

        {
            var currentDepartment = departmentDto.Department;
            EmployeeEntity soleManager = context.Employees.FirstOrDefault(x => x.Name == departmentDto.Manager) ??
            throw new Exception("Manager chosen does not exist in Employee database.");

            foreach (var employee in context.Employees.Where(x=> x.Department == currentDepartment && x.Type == EmployeeLevel.Manager))
            {
                employee.Type = EmployeeLevel.Employee;
                
            }
            
            soleManager.Type = EmployeeLevel.Manager;
            soleManager.Department = currentDepartment;

            context.SaveChanges();

        }
        
         public void UpdateDepartmentNameForDepartmentEmployees (DataContext context, string oldName, string newName)
        {
            foreach (var employee in context.Employees.Where(x=> x.Department == oldName))
            {
                employee.Department = newName;
            }

            context.SaveChanges();
        }
    }
}       