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

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lecturer model)
        {
            Console.WriteLine($"➡ DepartmentId nhận được: {model.DepartmentId}");
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", model.DepartmentId);
                return View(model);
            }
            _context.Lecturers.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var lecturer = _context.Lecturers.Find(id);
            if (lecturer == null)
                return NotFound();
            _context.Lecturers.Remove(lecturer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
