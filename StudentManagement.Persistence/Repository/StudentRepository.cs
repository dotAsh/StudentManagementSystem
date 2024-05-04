//Developed by Md. Ashik
using StudentManagement.Persistence.Data;
using StudentManagement.Persistence.Models;
using StudentManagement.Persistence.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace StudentManagement.Persistence.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _db;
        
        public StudentRepository(ApplicationDbContext db): base(db) 
        {
            _db = db;
            
        }
       

 
        public async Task<Student> UpdateAsync(Student entity)
        {
            entity.UpdatedDate = DateTimeOffset.UtcNow;
            _db.students.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
