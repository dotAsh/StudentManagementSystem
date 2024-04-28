using StudentManagement.Persistence.Models;
using StudentManagement.Persistence.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Service
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetAllStudentsAsync(int pageSize, int pageNumber);
        Task<StudentDTO> GetStudentByIdAsync(int id);

        Task<Student> CreateStudentAsync(StudentCreateDTO createDTO);
        Task<bool> DeleteStudentAsync(StudentDTO studentDTO);
        Task<bool> UpdateStudentAsync(StudentUpdateDTO updateDTO);
    }

}
