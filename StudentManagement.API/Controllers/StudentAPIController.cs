using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using StudentManagement.Persistence.Models;
using StudentManagement.Persistence.Models.DTO;
using StudentManagement.Persistence.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace StudentManagement.API.Controllers
{

    [Authorize]
    //[Route("api/[controller]")] //for this line class r name change korle route o change hobe which can be a problem
    [Route("api/StudentAPI")]
    //built in support for data annotation in StudentDTOs
    [ApiController]//some of the basic features are included by this like validations in StudetnDTO are effective here 
    public class StudentAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IStudentRepository _dbStudent;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentAPIController> _logger;
        public StudentAPIController(IStudentRepository dbStudent, IMapper mapper, ILogger<StudentAPIController> logger)
        {
            _dbStudent = dbStudent;
            _mapper = mapper;
            _response = new();
            _logger = logger;
        }



        [ResponseCache(Duration = 30)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetStudents(int pageSize= 3,int  pageNumber = 1)
        {
            try
            {

                IEnumerable<Student> studentList = await _dbStudent.GetAllAsync(pageSize:pageSize,pageNumber: pageNumber);
                _response.Result = _mapper.Map<List<StudentDTO>>(studentList);
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
                var student = await _dbStudent.GetAsync(x => x.Id == id);
                if (student == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<StudentDTO>(student);
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
        public async Task<ActionResult<APIResponse>> CreateStudent([FromBody] StudentCreateDTO createDTO)
        {
            try
            {
                //class r uper [ApiController] attribute na add korle ai if lagbe
                //custom validation r jonno o lagbe
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

                Student student = _mapper.Map<Student>(createDTO);


                await _dbStudent.CreateAsync(student);
                _response.Result = student;
                _response.StatusCode = HttpStatusCode.Created;
                //id will be populated auto
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

                var student = await _dbStudent.GetAsync(x => x.Id == id);
                if (student == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                await _dbStudent.RemoveAsync(student);
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
        [HttpPut("{id:int}", Name = "StudentUpdate")]
        public async Task<ActionResult<APIResponse>> VillaUpdate(int id, StudentUpdateDTO updateDTO)
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
                Student model = _mapper.Map<Student>(updateDTO);


                await _dbStudent.UpdateAsync(model);

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

/*
 * 
 using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using StudentManagement.Persistence.Models;
using StudentManagement.Persistence.Models.DTO;
using StudentManagement.Persistence.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;
using StudentManagement.Service;

namespace StudentManagement.API.Controllers
{

    [Authorize]
    //[Route("api/[controller]")] //for this line class r name change korle route o change hobe which can be a problem
    [Route("api/StudentAPI")]
    //built in support for data annotation in StudentDTOs
    [ApiController]//some of the basic features are included by this like validations in StudetnDTO are effective here 
    public class StudentAPIController : ControllerBase
    {
        //protected APIResponse _response;
        //private readonly IStudentRepository _dbStudent;
        //private readonly IMapper _mapper;
        //private readonly ILogger<StudentAPIController> _logger;
        //public StudentAPIController(IStudentRepository dbStudent, IMapper mapper, ILogger<StudentAPIController> logger)
        //{
        //    _dbStudent = dbStudent;
        //    _mapper = mapper;
        //    _response = new();
        //    _logger = logger;
        //}


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
        public async Task<ActionResult<APIResponse>> GetStudents(int pageSize= 3,int  pageNumber = 1)
        {
            try
            {

                IEnumerable<StudentDTO> studentDTOList = await _studentService.GetAllStudentsAsync(pageSize, pageNumber);
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
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                
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
        public async Task<ActionResult<APIResponse>> CreateStudent([FromBody] StudentCreateDTO createDTO)
        {
            try
            {
                //class r uper [ApiController] attribute na add korle ai if lagbe
                //custom validation r jonno o lagbe
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

                //Student student = _mapper.Map<Student>(createDTO);


                //await _dbStudent.CreateAsync(student);
                Student student = await _studentService.CreateStudentAsync(createDTO); 
                _response.Result = student;
                _response.StatusCode = HttpStatusCode.Created;
                //id will be populated auto
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

                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
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
        [HttpPut("{id:int}", Name = "StudentUpdate")]
        public async Task<ActionResult<APIResponse>> VillaUpdate(int id, StudentUpdateDTO updateDTO)
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



*/

