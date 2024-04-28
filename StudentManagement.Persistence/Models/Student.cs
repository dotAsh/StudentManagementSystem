using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Persistence.Models
{
    public class Student
    {
       

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }

        public DateTimeOffset EnrollmentDate { get; set; }

        //these properties will not be shown to the Client 
        public DateTimeOffset CreatedDate { get; set; }
        

        public DateTimeOffset UpdatedDate { get; set; }
    }
}


