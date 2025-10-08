namespace QuanLySinhVien_BTL.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public int phoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public Major major { get; set; }
    }
}
