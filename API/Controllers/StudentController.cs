using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBusinessLogic;
using StudentDataAccess;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudent()  // not static
        {
            List<StudentDTO> students = new List<StudentDTO>();

            students = Student.GetAllStudent();
            if (students == null || students.Count == 0)
            {
                return NotFound("No Students");
            }
            else
                return Ok(students);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudent()  // not static
        {
            List<StudentDTO> students = new List<StudentDTO>();

            students = Student.GetPassedStudents();
            if (students == null || students.Count == 0)
            {
                return NotFound("No Students");
            }
            else
                return Ok(students);
        }

        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Double> GetAverageGrade()
        {
            double Avg = Student.GetAverageGrade();
            return Ok(Avg);
        }


        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id < 0) return BadRequest($"Not accepted ID {id}");

            Student S = Student.Find(id);

            if (S == null)
                return NotFound($"Student with ID {id} not found.");

            StudentDTO SDTO = S.SDTO;

            return Ok(SDTO);

        }



        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<StudentDTO> UpdateStudent(int id, StudentDTO updatedStudent)
        {
            if(id < 0 || updatedStudent==null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
                return BadRequest("Invalid student data.");

            Student student = Student.Find(id);

            if (student == null) return NotFound($"Student with ID {id} not found.");


            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;

            if (!student.Save())
                return StatusCode(500, "Failed to update student.");
            
       
                return Ok(student.SDTO);

        }


        [HttpPost( Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<StudentDTO> AddStudent( StudentDTO NewStudent)
        {
            if ( NewStudent == null || string.IsNullOrEmpty(NewStudent.Name) || NewStudent.Age < 0 || NewStudent.Grade < 0)
                return BadRequest("Invalid student data.");

            Student student = new Student(NewStudent);


            if (!student.Save())
                return StatusCode(500, "Failed to Add student.");

                NewStudent.Id = student.ID;
        
                return CreatedAtRoute("GetStudentById", new {id = NewStudent.Id } , NewStudent );

        }


        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult DeleteStudent(int id)
        {
            if (id < 0) return BadRequest($"Not accepted ID {id}");


            if (Student.DeleteStudent(id))
                return Ok($"Student with ID {id} has been deleted.");
            else
                return NotFound($"Student with ID {id} not found. no rows deleted!");

        }

    }
    }
