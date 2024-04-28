using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Persistence.Models.DTO
{
    public class StudentDTO
    {
       
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }

        public DateTimeOffset EnrollmentDate { get; set; }


    }
}
