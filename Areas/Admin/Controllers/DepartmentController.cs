using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using QuanLySinhVien_BTL.Data;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, string DepartmentId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentDepartmentId"] = DepartmentId;

            var departments = _context.Departments.AsQueryable();  

            if (!string.IsNullOrEmpty(searchString))
            {
                departments = departments.Where(d => d.DepartmentId.Contains(searchString) || d.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(DepartmentId))
            {
                departments = departments.Where(d => d.DepartmentId == DepartmentId);
            }

            return View(departments.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Department());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department model)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
