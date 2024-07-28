using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class DepartmentDto
    {
        public required string Department { get; set; }
        public int NumOfEmployee { get; set; } // set to 0 if not required?
        public required string? Manager { get; set; }
    }
}