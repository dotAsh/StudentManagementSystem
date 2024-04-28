using StudentManagement.Persistence.Models;
using System.Linq.Expressions;

namespace StudentManagement.Persistence.Repository.IRepository
{
    public interface IStudentRepository: IRepository<Student>
    {
       

        Task<Student> UpdateAsync(Student entity);

        


    }
}
