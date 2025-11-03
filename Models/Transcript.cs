using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Transcript
    {
        [Key]
        public int TranscriptId { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public double? ProcessGrade { get; set; }
        public double? FinalGrade { get; set; }

        [NotMapped]
        public double? GPA { get; set; }
    }
}
