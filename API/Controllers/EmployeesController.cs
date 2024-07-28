using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class EmployeesController(DataContext context, IContextService contextService) : BaseController
    {

        [HttpGet("getdel")]
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> GetAllllEmployees() {

            return await context.Employees.ToListAsync();

        }

        [HttpGet("restore")]
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> Restore() {

            foreach (var employee in context.Employees){
                employee.IsDeleted = false;
            }

            await context.SaveChangesAsync();
            contextService.ConfigureNumOfEmployeesDepartmentsAndManagers(context);
            await context.SaveChangesAsync();

            return await context.Employees.ToListAsync();

        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> GetAllEmployees() {

            return await context.Employees.Where(x => x.IsDeleted == false).ToListAsync();

        }

        [HttpPost("post")]
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> Post(EmployeeDto employeeDto)
        {

            if (employeeDto.Name == "" || employeeDto.Department == "")
            {
                return BadRequest("Please fill all fields");
            }

            var newEmployee = new EmployeeEntity 
            {
                Name = employeeDto.Name,
                Type = employeeDto.Type,
                Department = employeeDto.Department
            };

            if (employeeDto.Type == EmployeeLevel.Manager && 
                    contextService.CheckIfManagerExists(context, employeeDto, newEmployee.Id))
            {
                return BadRequest("Managaer for this department already exists.");
            }

            bool nameExists = await context.Employees.AnyAsync(e => e.Name == newEmployee.Name && e.IsDeleted == false);
            if  (nameExists) 
            {
                return BadRequest("Name already exists in database");
            }

            context.Employees.Add(newEmployee);
        
            await context.SaveChangesAsync();
            contextService.ConfigureNumOfEmployeesDepartmentsAndManagers(context);
            await context.SaveChangesAsync();


            return await context.Employees.Where(x=> x.IsDeleted == false).ToListAsync();
        }


        [HttpPut("put/{id}")]
        
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> UpdateUser(int id, EmployeeDto employeeDto){
            
            if (employeeDto.Name == "" || employeeDto.Department == "")
            {
                return BadRequest("Please fill all fields");
            }
            
            EmployeeEntity EmployeeToBeUpdated = await context.Employees.FindAsync(id) 
            ?? throw new Exception("Employee at given Id does not exist.");

            if (employeeDto.Type == EmployeeLevel.Manager && 
                    contextService.CheckIfManagerExists(context, employeeDto, id))
            {
                return BadRequest("Managaer for this department already exists.");
            }

            bool nameExists = await context.Employees.AnyAsync(e => e.Name == employeeDto.Name && e.Id != id && e.IsDeleted == false);
            if  (nameExists) 
            {
                return BadRequest("Name already exists in database");
            }

            EmployeeToBeUpdated.Name = employeeDto.Name;
            EmployeeToBeUpdated.Type = employeeDto.Type;
            EmployeeToBeUpdated.Department = employeeDto.Department;
            
            await context.SaveChangesAsync();
            contextService.ConfigureNumOfEmployeesDepartmentsAndManagers(context);
            await context.SaveChangesAsync();

            return await context.Employees.Where(x=> x.IsDeleted == false).ToListAsync();
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> DeleteName(int id) 
        {
            EmployeeEntity EmployeeToBeDeleted = await context.Employees.FindAsync(id) 
            ?? throw new Exception("Given ID does not exist");

            EmployeeToBeDeleted.IsDeleted = true;
            await context.SaveChangesAsync();
            contextService.ConfigureNumOfEmployeesDepartmentsAndManagers(context);
            await context.SaveChangesAsync();

            return await context.Employees.Where(x=> x.IsDeleted == false).ToListAsync();

        }

        [HttpGet("get/names")]

        public ActionResult<string[]> getAllNames()
        {
            string[] names = [];
            List<string> namesList = names.ToList();
            foreach(var employee in context.Employees.Where(x=> x.IsDeleted == false))
            {
                namesList.Add(employee.Name);
            }
            names = namesList.ToArray();

            return names;
        }
        


        [HttpGet("length")]
        public int GetLength()
        {
            var length = 0;
            foreach(var value in context.Employees.Where(x => x.IsDeleted == false)){
                length++;
            }

            return length;
        }
    }
}