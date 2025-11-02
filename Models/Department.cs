using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien_BTL.Models
{
    public class Department
    {
        [Key]
        public string DepartmentId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

<<<<<<< HEAD
        public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
        public ICollection<Major> Majors { get; set; } = new List<Major>();
=======
        public ICollection<Major>? Majors { get; set; }
        public ICollection<Lecturer>? Lecturers { get; set; }
>>>>>>> 8a75d956658d41add0c2c0d9227d15bd63ad3e74
    }
}
