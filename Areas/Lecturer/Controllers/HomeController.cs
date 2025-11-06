using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using QuanLySinhVien_BTL.ViewModels;


namespace QuanLySinhVien_BTL.Areas.Lecturer.Controllers
{
    [Area("Lecturer")]
    [Authorize(Roles = "Giảng Viên")]
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
            string displayName = "Giảng viên";
            if (user != null)
            {
                // Tìm giảng viên có UserId trùng với Id của user
                var lecturer = await _context.Lecturers
                    .FirstOrDefaultAsync(l => l.UserId == user.Id);
                if (lecturer != null)
                {
                    displayName = lecturer.Name;
                }
                else
                {
                    // fallback: nếu không có lecturer -> dùng tên đăng nhập
                    displayName = user.UserName;
                }
            }
            // Truyền vào ViewModel
            var model = new UserViewModel
            {
                DisplayName = displayName
            };

            return View(model);
        }
    }
}