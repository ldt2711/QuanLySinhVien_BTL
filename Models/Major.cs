namespace QuanLySinhVien_BTL.Models
{
    public class Major
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public int TotalCredits { get; set; }

        // 🔹 Mối quan hệ nhiều–nhiều với Student
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
