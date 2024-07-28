using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class EmployeeDto
    {
        public required string Name { get; set; }
        public required EmployeeLevel Type { get; set; }
        public required string? Department { get; set; }
    }
}