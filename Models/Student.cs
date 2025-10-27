using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string phoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Trường Ngành học là bắt buộc.")]
        [Display(Name = "Ngành học")]
        public string MajorCode { get; set; }

        // 🔹 Thuộc tính điều hướng (navigation property)
        [ValidateNever]
        public Major Major { get; set; }
    }
}
