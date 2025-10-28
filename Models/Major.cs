using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Major
    {
        [Key]
        public string MajorId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [ForeignKey("Department")]
        public string DepartmentId { get; set; }
        public Department? Department { get; set; }

        
        public ICollection<Course>? Courses { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
