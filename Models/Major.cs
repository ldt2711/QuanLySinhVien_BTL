using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Major
    {
        [Key]
        [Required(ErrorMessage = "Mã ngành là bắt buộc.")]
        public string MajorId { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Tên ngành là bắt buộc.")]
        public string Name { get; set; }
        [ForeignKey("Department")]
        [Required(ErrorMessage = "Trường Khoa là bắt buộc.")]
        public string DepartmentId { get; set; }
        public Department? Department { get; set; }

        
        public ICollection<Course>? Courses { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
