//using AutoMapper;
using StudentManagement.Persistence.Models;
using StudentManagement.Service.DTO;
using StudentManagement.Persistence.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Service.Mappers;

namespace StudentManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        //private readonly IMapper _mapper;

        //public StudentService(IStudentRepository studentRepository, IMapper mapper)
         public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            //_mapper = mapper;
        }

        public async Task<List<StudentDTO>> GetAllStudentsAsync(int pageSize, int pageNumber)
        {
            List<Student> studentList = await _studentRepository.GetAllAsync(pageSize:pageSize, pageNumber:pageNumber);
            //return  _mapper.Map<List<StudentDTO>>(studentList);
            return studentList.MapToDTOList();
        }
      
        public async Task<StudentDTO> CreateStudentAsync(StudentCreateDTO createDTO)
        {
            try
            {
                //Student student = _mapper.Map<Student>(createDTO);

                Student student = createDTO.MapToStudent();
                await _studentRepository.CreateAsync(student);

                //return _mapper.Map<StudentDTO>(student);

                return student.MapToDto();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        async Task<bool> IStudentService.DeleteStudentAsync(StudentDTO studentDTO)
        {
            try
            {
                //Student student = _mapper.Map<Student>(studentDTO);
                Student student = studentDTO.MapToDomain();
                await _studentRepository.RemoveAsync(student);
                return true;
            }
            catch
            {
                throw new Exception();
            }
        }

        
        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
            
            try
            {
                var student = await _studentRepository.GetAsync(x => x.Id == id,false);
                if (student == null)
                {
                    return null;
                }
                //return _mapper.Map<StudentDTO>(student);
                return student.MapToDto();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<bool> IStudentService.UpdateStudentAsync( StudentUpdateDTO updateDTO)
        {
            try
            {
                //Student model = _mapper.Map<Student>(updateDTO);
                Student model = updateDTO.MapToStudent();

                await _studentRepository.UpdateAsync(model);
                return true;
            }catch (Exception ex) {
                throw ex;
            }
            
        }

      
    }

}
