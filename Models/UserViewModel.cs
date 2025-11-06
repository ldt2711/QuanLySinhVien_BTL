using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien_BTL.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // For create/edit
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
        public string DisplayName {  get; set; } = string.Empty;
    }
}
