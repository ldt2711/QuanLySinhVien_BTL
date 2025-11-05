using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Tên lớp học phần là bắt buộc.")]
        public string CourseName { get; set; }
        [ForeignKey("Major")]
        [Required(ErrorMessage = "Phải chọn ngành học.")]
        public string MajorId { get; set; }
        public Major? Major { get; set; }
        [ForeignKey("Lecturer")]
        [Required]
        public int LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }
        [StringLength(20)]
        [Required(ErrorMessage = "Học kỳ là bắt buộc.")]
        [RegularExpression(@"^([1-9]|1[0-9]|20)$", ErrorMessage = "Học kỳ chỉ được đến 20")]
        public string Semester { get; set; }
        [StringLength(20)]
        [Required(ErrorMessage = "Năm học là bắt buộc.")]
        [RegularExpression(@"^(20\\d{2})$", ErrorMessage = "Năm học phải có định dạng 'YYYY'.")]
        public string Year { get; set; }
        [Required(ErrorMessage = "Hệ số phải nhập")]
        [RegularExpression(@"^(0(\\.\\d{1})?)$", ErrorMessage = "Hệ số phải từ nhỏ hơn 1 và có tối đa một chữ số thập phân.")]
        public float Coefficient { get; set; }
        public ICollection<Transcript>? Transcripts { get; set; }

    }
}
