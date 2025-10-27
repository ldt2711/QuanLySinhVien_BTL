using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace QuanLySinhVien_BTL.Models
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DepartmentCode { get; set; }
        [ValidateNever]
        public Department Department { get; set; }

    }
}
