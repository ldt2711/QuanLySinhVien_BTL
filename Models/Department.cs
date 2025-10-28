using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien_BTL.Models
{
    public class Department
    {
        [Key]
        public string DepartmentCode { get; set; }
        public string Name { get; set; }
        public DateTime DateOfEstablishment { get; set; }

        public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
    }
}
