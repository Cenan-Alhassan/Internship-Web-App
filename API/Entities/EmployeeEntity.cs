using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public enum EmployeeLevel 
    {
        Manager = 0,
        Employee = 1,
        HR = 2
    }
    public class EmployeeEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required EmployeeLevel Type { get; set; }
        public required string? Department { get; set; }
        public bool IsDeleted { get; set; }

    }
}