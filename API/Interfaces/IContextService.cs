using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IContextService
    {
        void ConfigureNumOfEmployeesDepartmentsAndManagers (DataContext context);

        bool CheckIfManagerExists (DataContext context, EmployeeDto employeeDto, int Id);

        void SetAsSoleManager (DataContext context, DepartmentDto departmentDto);

        void UpdateDepartmentNameForDepartmentEmployees (DataContext context, string oldName, string newName);
    }
}