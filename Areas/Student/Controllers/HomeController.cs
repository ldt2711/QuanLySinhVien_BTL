using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using System.Threading.Tasks;

namespace QuanLySinhVien_BTL.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Sinh Viên")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            string displayName = "Sinh viên";

            if (user != null)
            {
                // Tìm sinh viên có UserId trùng v?i Id c?a user
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.UserId == user.Id);

                if (student != null)
                {
                    displayName = student.Name;
                }
                else
                {
                    // fallback: n?u không có student -> dùng tên ??ng nh?p
                    displayName = user.UserName;
                }
            }

            // Truy?n vào ViewModel
            var model = new UserViewModel
            {
                DisplayName = displayName
            };


            return View(model);
        }
    }
}