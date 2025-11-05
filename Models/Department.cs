using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien_BTL.Models
{
    public class Department
    {
        [Key]
        [Required(ErrorMessage = "Mã khoa là bắt buộc.")]
        public string DepartmentId { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Tên khoa là bắt buộc.")]
        public string Name { get; set; }

        public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
        public ICollection<Major> Majors { get; set; } = new List<Major>();

    }
}
