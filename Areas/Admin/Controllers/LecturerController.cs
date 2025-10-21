using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var lecturers = _context.Lecturers
                .Include(l => l.Department)
                .ToList();
            return View(lecturers);
        }
    }
}
