using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using QuanLySinhVien_BTL.Models;
using System.Threading.Tasks;

namespace QuanLySinhVien_BTL.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Sinh Viên")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
       public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }
    }
}