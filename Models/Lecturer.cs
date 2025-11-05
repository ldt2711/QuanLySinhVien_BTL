using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Lecturer
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Tên giảng viên là bắt buộc.")]
        [RegularExpression(@"^[A-Za-zÀ-ỹ\s]+$", ErrorMessage = "Họ tên chỉ được chứa chữ cái và khoảng trắng.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phải chọn giới tính.")]
        public Gender Gender { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@qlsv+\.com$", ErrorMessage = "Email phải có dạng example@qlsv.com.")]
        public string? Email { get; set; }
        [StringLength(20)]
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? Phone { get; set; }

        [ForeignKey("Department")]
        [Required(ErrorMessage = "Phải chọn khoa.")]
        public string DepartmentId { get; set; }
        public Department? Department { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Course>? Courses { get; set; }
    }
}
