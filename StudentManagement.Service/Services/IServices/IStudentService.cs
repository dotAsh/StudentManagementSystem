using StudentManagement.Persistence.Models;
using StudentManagement.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Service.Services.IServices
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetAllStudentsAsync(string filterBy = null, string sortBy = null, int pageSize = 10, int pageNumber = 1);
        Task<StudentDTO> GetStudentByIdAsync(int id);

        Task<StudentDTO> CreateStudentAsync(StudentCreateDTO createDTO);
        Task<bool> DeleteStudentAsync(StudentDTO studentDTO);
        Task<bool> UpdateStudentAsync(StudentUpdateDTO updateDTO);
    }

}
