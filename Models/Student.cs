using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        [ForeignKey("Major")]
        [Required(ErrorMessage = "Trường Ngành học là bắt buộc.")]
        [Display(Name = "Ngành học")]
        public string MajorId { get; set; }
        public Major? Major { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Transcript>? Transcripts { get; set; }
    }
}
