using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LecturerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString, string DepartmentId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentDepartmentId"] = DepartmentId;

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "Name", DepartmentId);

            var lecturers = _context.Lecturers
                .Include(l => l.Department)
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int lecturerId))
                {
                    lecturers = lecturers.Where(l => l.Id == lecturerId || l.Name.Contains(searchString));
                }
                else
                {
                    lecturers = lecturers.Where(l => l.Name.Contains(searchString));
                }
            }

            // Lọc theo Khoa
            if (!string.IsNullOrEmpty(DepartmentId))
            {
                lecturers = lecturers.Where(l => l.DepartmentId == DepartmentId);
            }

            return View(await lecturers.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "Name");
            return View(new Models.Lecturer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Lecturer model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

                var email = model.Email;
                var password = email.Split('@')[0];

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    model.UserId = user.Id;
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(user, "Giảng Viên");
                }
                else
                {
                    ModelState.AddModelError("", string.Join(";", result.Errors.Select(e => e.Description)));
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "Name", model.DepartmentId);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
                return NotFound();

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "Name", lecturer.DepartmentId);
            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,Email,Phone,DepartmentId")] Models.Lecturer lecturer)
        {
            if (id != lecturer.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "Name", lecturer.DepartmentId);
            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(lecturer.UserId))
            {
                var user = await _userManager.FindByIdAsync(lecturer.UserId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.Id == id);
        }
    }
}
