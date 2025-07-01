using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Infrastructure.Data;

namespace UniversitySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(FacultyDbContext facultyContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            try
            {
                var students = facultyContext.Students.ToList();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] Domain.Models.Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.FullName))
            {
                return BadRequest(new { Error = "Invalid student data." });
            }
            try
            {
                facultyContext.Students.Add(student);
                facultyContext.SaveChanges();
                return CreatedAtAction(nameof(GetStudents), new { id = student.Id }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }
    }
}
