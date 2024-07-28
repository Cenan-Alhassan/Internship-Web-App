using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class DepartmentEntity
    {
        public int Id { get; set; }
        public required string Department { get; set; }
        public int NumOfEmployee { get; set; } // set to 0 if created without
        public required string? Manager { get; set; }
        public bool IsDeleted { get; set; } // set to false if created without



    }
}