using Microsoft.Identity.Client;

namespace QuanLySinhVien_BTL.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfEstablishment { get; set; }

        public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
    }
}
