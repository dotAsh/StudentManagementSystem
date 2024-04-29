using StudentManagement.Persistence.Models;
using StudentManagement.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Service.Mappers
{
    public static class Mapper
    {
        public static StudentDTO MapToDto(this Student student)
        {
            if (student == null)
            {
                return null;

            }
                

            return new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Gender = student.Gender,
                Email = student.Email,
                Department = student.Department,
                EnrollmentDate = student.EnrollmentDate
            };
        }

        public static Student MapToDomain(this StudentDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return null;
            }
            return new Student
            {
                Id = studentDTO.Id,
                Name = studentDTO.Name,
                Gender = studentDTO.Gender,
                Email = studentDTO.Email,
                Department = studentDTO.Department,
                EnrollmentDate = studentDTO.EnrollmentDate
            };
        }

        public static List<StudentDTO> MapToDTOList(this IEnumerable<Student> students)
        {
            var studentDTOList = new List<StudentDTO>();
            foreach (var student in students)
            {
                studentDTOList.Add(student.MapToDto());
            }
            return studentDTOList;
        }

        public static Student MapToStudent(this StudentCreateDTO createDTO)
        {
            if (createDTO == null)
                return null;

            return new Student
            {
                Name = createDTO.Name,
                Gender = createDTO.Gender,
                Email = createDTO.Email,
                Department = createDTO.Department,
                EnrollmentDate = createDTO.EnrollmentDate,
                CreatedDate = DateTimeOffset.UtcNow
           
            };
        }

        public static Student MapToStudent(this StudentUpdateDTO updateDTO)
        {
            if (updateDTO == null)
                return null;

            return new Student
            {
                Id = updateDTO.Id,
                Name = updateDTO.Name,
                Gender = updateDTO.Gender,
                Email = updateDTO.Email,
                Department = updateDTO.Department,
                EnrollmentDate = updateDTO.EnrollmentDate,
                UpdatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
