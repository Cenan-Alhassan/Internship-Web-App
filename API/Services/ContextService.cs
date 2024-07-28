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
        public void ConfigureNumOfEmployeesDepartmentsAndManagers (DataContext context)
        {

            foreach(var departmentEntity in context.Departments)
            {
                var DepartmentName = departmentEntity.Department;
                var NumOfEmployee = 0;
                string Manager = "none";

                foreach (var employee in context.Employees.Where(x=> x.Department == DepartmentName && x.IsDeleted == false))
                {
                    // increment for every employee in department
                    NumOfEmployee++;

                    // find the manager of the department
                    if (employee.Type == EmployeeLevel.Manager)
                    {
                        Manager = employee.Name;
                    }

                    
                }
                


                // DepartmentEntity department = context.Departments.FirstOrDefault(x=> x.Department == DepartmentName)
                // ?? throw new Exception("Cannot configure number of employees for non-existant department.");

                departmentEntity.NumOfEmployee = NumOfEmployee;
                departmentEntity.Manager = Manager;
                context.SaveChanges();

            }

            // set employee department to none if department no longer exists or was edited away 
            foreach (var employee in context.Employees)
                {
                    var departmentName = "none";
                    
                    foreach (var department in context.Departments.Where(x=> x.IsDeleted == false))
                    {
                        if (employee.Department == department.Department)
                        {
                            departmentName = department.Department;
                        }
                    }

                    employee.Department = departmentName;
                    context.SaveChanges();
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