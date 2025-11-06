using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien_BTL.ViewModels
{
    public class StudentEditViewModel
    {
        public int Id { get; set; } // Cần Id để xác định sinh viên nào đang được sửa

        // Các trường không cho sửa nhưng muốn hiển thị
        public string Name { get; set; }
        public string Email { get; set; }
        public string MajorName { get; set; } // Hiển thị tên ngành thay vì MajorId

        // Các trường cho phép sửa
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 15 ký tự.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa ký tự số.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự.")]
        public string Address { get; set; }
    }
}