using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Transcript
    {
        [Key]
        public int TranscriptId { get; set; }
        [ForeignKey("Student")]
        [Required]  
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        [ForeignKey("Course")]
        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        [Required(ErrorMessage = "Phải nhập điểm quá trình.")]
        [RegularExpression(@"^(10(\.0{1})?|[0-9]{1,2}(\.[0-9]{1,2})?)$", ErrorMessage = "Điểm quá trình phải từ 0 đến 10 và có tối đa một chữ số thập phân.")]
        public double? ProcessGrade { get; set; }
        [Required(ErrorMessage = "Phải nhập điểm cuối kỳ.")]
        [RegularExpression(@"^(10(\.0{1})?|[0-9]{1}(\.[0-9]{1})?)$", ErrorMessage = "Điểm cuối kỳ phải từ 0 đến 10 và có tối đa một chữ số thập phân.")]
        public double? FinalGrade { get; set; }

        [NotMapped]
        public double? GPA { get; set; }
    }
}
