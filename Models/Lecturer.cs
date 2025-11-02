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
        public string Name { get; set; }
        public Gender Gender { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(20)]
        public string? Phone { get; set; }

        [ForeignKey("Department")]
        public string DepartmentId { get; set; }
        public Department? Department { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Course>? Courses { get; set; }
    }
}
