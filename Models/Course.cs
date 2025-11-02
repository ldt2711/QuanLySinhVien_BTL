using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien_BTL.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [StringLength(100)]
        public string CourseName { get; set; }
        [ForeignKey("Major")]
        public string MajorId { get; set; }
        public Major? Major { get; set; }
        [ForeignKey("Lecturer")]
        public int LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }
        [StringLength(20)]
        public string Semester { get; set; }
        [StringLength(20)]
        public string Year { get; set; }
        [Required]
        public float Coefficient { get; set; }
        public ICollection<Transcript>? Transcripts { get; set; }

    }
}
