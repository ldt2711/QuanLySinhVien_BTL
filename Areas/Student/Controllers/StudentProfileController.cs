using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien_BTL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLySinhVien_BTL.Areas.Student.Controllers
{
    [Area("Student")]
    public class StudentProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var student = _context.Students
                .Include(s => s.Major)
                .FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
    }
}
