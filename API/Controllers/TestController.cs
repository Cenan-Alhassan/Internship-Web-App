using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly string _dbPath;

    public TestController(IWebHostEnvironment env)
    {
        _dbPath = Path.Combine(env.ContentRootPath, "EmployeeDepartments.db");
    }

    [HttpGet("check-db-access")]
    public IActionResult CheckDbAccess()
    {
        try
        {
            // Test reading from the database file
            if (!System.IO.File.Exists(_dbPath))
            {
                return NotFound("Database file not found.");
            }

            // Try opening the file for read/write access
            using (var stream = new FileStream(_dbPath, FileMode.Open, FileAccess.ReadWrite))
            {
                return Ok("Database file is accessible for read/write operations.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error accessing database file: {ex.Message}");
        }
    }

    // [HttpGet("test")]
    // public string Test()
    // {
    //     return Ok("reading");
    // }
}
}