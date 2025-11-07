using QuanLySinhVien_BTL.Models;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien_BTL.ViewModels 
{
    public class LecturerEditViewModel
    {
    public int Id { get; set; } // Cần Id để xác định giảng viên nào đang được sửa
                                // Các trường không cho sửa nhưng muốn hiển thị
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public string DepartmentName { get; set; }
    public string Email { get; set; }

    // Các trường cho phép sửa
    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa ký tự số.")]
    public string Phone { get; set; }
    }
}
