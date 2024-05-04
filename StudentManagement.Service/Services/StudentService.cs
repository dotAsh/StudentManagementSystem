//Developed by Md. Ashik
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
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentManagement.Service.Services.IServices;
//Developed by Md. Ashik
namespace StudentManagement.Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<StudentDTO>> GetAllStudentsAsync(string filterBy = null, string sortBy = null, int pageSize = 10, int pageNumber = 1)
        {
            List<Student> studentList;
            if (filterBy != null)
            {
                studentList = await _studentRepository.GetAllAsync(filter: u => u.Name.Contains(filterBy), pageSize: pageSize, pageNumber: pageNumber);
            }
            else
            {
                studentList = await _studentRepository.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
            }

            if (!string.IsNullOrEmpty(sortBy) && studentList != null)
            {
                var property = typeof(Student).GetProperty(sortBy);
                if (property != null)
                {

                    studentList = studentList.OrderBy(x => property.GetValue(x)).ToList();
                }
            }
            return studentList.MapToDTOList();
        }

        public async Task<StudentDTO> CreateStudentAsync(StudentCreateDTO createDTO)
        {
            try
            {
                Student student = createDTO.MapToStudent();
                await _studentRepository.CreateAsync(student);

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
                var student = await _studentRepository.GetAsync(x => x.Id == id, false);
                if (student == null)
                {
                    return null;
                }

                return student.MapToDto();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<bool> IStudentService.UpdateStudentAsync(StudentUpdateDTO updateDTO)
        {
            try
            {
                Student model = updateDTO.MapToStudent();

                await _studentRepository.UpdateAsync(model);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }

}
