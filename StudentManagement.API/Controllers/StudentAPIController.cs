
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Service.DTO;
using StudentManagement.Persistence.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;
using StudentManagement.API.Response;
using StudentManagement.API.Filters;
using System.Globalization;
using StudentManagement.Service.Services.IServices;
//Developed by Md. Ashik

namespace StudentManagement.API.Controllers
{

    
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [Authorize]
    [Route("api/StudentAPI")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentAPIController> _logger;
        protected APIResponse _response;
        public StudentAPIController(IStudentService studentService, ILogger<StudentAPIController> logger)
        {
            _studentService = studentService;
            _logger = logger;
            _response = new();
        }

        [ResponseCache(Duration = 30)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> GetStudents(int pageSize= 10,int  pageNumber = 1,string filterBy = null, string sortBy = "Name")
        {
            try
            {

                IEnumerable<StudentDTO> studentDTOList = await _studentService.GetAllStudentsAsync( filterBy:filterBy,  sortBy:sortBy,pageSize:pageSize, pageNumber:pageNumber);
                
                _response.Result = studentDTOList;
                _response.StatusCode = HttpStatusCode.OK;
                _logger.LogInformation("getting all students");
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error :" + ex);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> GetStudent(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _logger.LogError("Get student error with Id " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                StudentDTO student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Student not found." };
                    return NotFound(_response);
                }
                _response.Result = student;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);

            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> CreateStudent([FromBody] StudentCreateDTO createDTO)
        {
            try
            {
        
                if (!ModelState.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = ModelState;
                    return BadRequest(_response);

                }


                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    return BadRequest(createDTO);
                }

           
                StudentDTO student = await _studentService.CreateStudentAsync(createDTO); 
                _response.Result = student;
                _response.StatusCode = HttpStatusCode.Created;
                //id will be auto populated 
                return CreatedAtRoute("GetStudent", new { id = student.Id }, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [HttpDelete("{id:int}", Name = "DeleteStudent")]
        public async Task<ActionResult<APIResponse>> DeleteStudent(int id)
        {
            try
            {

                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);

                }

                StudentDTO student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Student not found for deletion." };
                    return NotFound(_response);

                }
                await _studentService.DeleteStudentAsync(student);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [HttpPut("{id:int}", Name = "StudentUpdate")]
        public async Task<ActionResult<APIResponse>> StudentUpdate(int id, StudentUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || updateDTO.Id != id)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Student Id Mismatched" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                StudentDTO student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "Student not found for update." };
                    return NotFound(_response);

                }



                await _studentService.UpdateStudentAsync(updateDTO);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }


        }


    }
}




