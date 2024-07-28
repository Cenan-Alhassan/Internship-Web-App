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
    public class DepartmentsController(DataContext context, IContextService contextService) : BaseController
    {

        [HttpGet("restore")]
        public async Task<ActionResult<IEnumerable<DepartmentEntity>>> Restore() {

            foreach (var department in context.Departments){
                department.IsDeleted = false;
            }
            await context.SaveChangesAsync();

            return await context.Departments.ToListAsync();

        }
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<DepartmentEntity>>> GetAllDepartments() {

            return await context.Departments.Where(x => x.IsDeleted == false).ToListAsync();

        }

        [HttpPost("post")]
        public async Task<ActionResult<IEnumerable<DepartmentEntity>>> Post(DepartmentDto DepartmentDto)
        {

            if (DepartmentDto.Manager == "" || DepartmentDto.Department == "")
            {
                return BadRequest("Please fill all fields");
            }
            
            var newDepartment = new DepartmentEntity 
            {
                Department = DepartmentDto.Department,
                Manager = DepartmentDto.Manager,
                NumOfEmployee = 0
            };

            bool DepartmentExists = await context.Departments.AnyAsync(e => e.Department == newDepartment.Department && e.IsDeleted == false);
            if  (DepartmentExists) 
            {
                return BadRequest("Department already exists in database.");
            }

            // bool ManagerExists = await context.Employees.AnyAsync(x => x.Name == newDepartment.Manager);
            // if (!ManagerExists)
            // {
            //     return BadRequest("Chosen Manager does not exist in Employee database.");
            // }
            contextService.SetAsSoleManager(context, DepartmentDto);
            context.Departments.Add(newDepartment);

            await context.SaveChangesAsync();
            contextService.ConfigureNumOfEmployeesDepartmentsAndManagers(context);
            await context.SaveChangesAsync();


            return await context.Departments.Where(x => x.IsDeleted == false).ToListAsync();
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<IEnumerable<DepartmentEntity>>> DeleteDepartment(int id) 
        {
            DepartmentEntity DepartmentToBeDeleted = await context.Departments.FindAsync(id) 
            ?? throw new Exception("Given ID does not exist");

            DepartmentToBeDeleted.IsDeleted = true;
            
            contextService.ConfigureNumOfEmployeesDepartmentsAndManagers(context);
            await context.SaveChangesAsync();

            return await context.Departments.Where(x => x.IsDeleted == false).ToListAsync();

        }


        [HttpPut("put/{id}")]
        
        public async Task<ActionResult<IEnumerable<DepartmentEntity>>> UpdateUser(int id, DepartmentDto DepartmentDto){

            DepartmentEntity DepartmentToBeUpdated = await context.Departments.FindAsync(id) 
            ?? throw new Exception("Department at given Id does not exist.");

            if (DepartmentDto.Manager == "" || DepartmentDto.Department == "")
            {
                return BadRequest("Please fill all fields");
            }

            var oldName = DepartmentToBeUpdated.Department;

            bool DepartmentExists = await context.Departments.AnyAsync(e => e.Department == DepartmentDto.Department && e.Id != id && e.IsDeleted == false);
            if  (DepartmentExists) 
            {
                return BadRequest("Department already exists in database.");
            }

            DepartmentToBeUpdated.Department = DepartmentDto.Department;
            DepartmentToBeUpdated.NumOfEmployee = DepartmentDto.NumOfEmployee;
            DepartmentToBeUpdated.Manager = DepartmentDto.Manager;
            
            contextService.UpdateDepartmentNameForDepartmentEmployees(context, oldName, DepartmentDto.Department);
            contextService.SetAsSoleManager(context, DepartmentDto);
            
            await context.SaveChangesAsync();
            contextService.ConfigureNumOfEmployeesDepartmentsAndManagers(context);
            await context.SaveChangesAsync();

            return await context.Departments.Where(x => x.IsDeleted == false).ToListAsync();
        }

        [HttpGet("length")]
        public ActionResult<int> GetLength()
        {
            var length = 0;
            foreach(var value in context.Departments.Where(x => x.IsDeleted == false)){
                length++;
            }

            return length;
        }

        [HttpGet("get/names")]
        public ActionResult<string[]> getAllNames()
        {
            string[] names = [];
            List<string> namesList = names.ToList();
            foreach(var department in context.Departments.Where(x=> x.IsDeleted == false))
            {
                namesList.Add(department.Department);
            }
            names = namesList.ToArray();

            return names;
        }
    }
}