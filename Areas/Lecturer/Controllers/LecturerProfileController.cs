using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using QuanLySinhVien_BTL.ViewModels;

namespace QuanLySinhVien_BTL.Areas.Lecturer.Controllers
{
    [Area("Lecturer")]
    [Authorize(Roles = "Giảng Viên")]
    public class LecturerProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LecturerProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MyProfile()
        {
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (CurrentUserId == null)
            {
                return Unauthorized();
            }

            var lecturer = await _context.Lecturers.Include(l => l.Department).FirstOrDefaultAsync(l => l.UserId == CurrentUserId);

            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId == null)
            {
                return Unauthorized();
            }
            var lecturer = await _context.Lecturers.Include(l => l.Department).FirstOrDefaultAsync(l => l.UserId == CurrentUserId);
            if (lecturer == null)
            {
                return NotFound();
            }

            var viewModel = new LecturerEditViewModel
            {
                Id = lecturer.Id,
                Name = lecturer.Name,
                Gender = lecturer.Gender,
                Email = lecturer.Email,
                DepartmentName = lecturer.Department?.Name, // Lấy tên khoa
                Phone = lecturer.Phone,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LecturerEditViewModel model)
        {
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (CurrentUserId == null)
            {
                return Unauthorized();
            }

            var LecturerToUpdate = await _context.Lecturers.FirstOrDefaultAsync(l => l.UserId == CurrentUserId);

            if (LecturerToUpdate == null)
            {
                return NotFound();
            }

            if (model.Id != LecturerToUpdate.Id)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                model.Name = LecturerToUpdate.Name;
                model.Email = LecturerToUpdate.Email;
                model.DepartmentName = (await _context.Departments.FindAsync(LecturerToUpdate.DepartmentId))?.Name;
                return View(model);
            }

            LecturerToUpdate.Phone = model.Phone;
            try
            {
                _context.Update(LecturerToUpdate);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(MyProfile));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LecturerExists(LecturerToUpdate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã xảy ra lỗi không mong muốn: {ex.Message}");
                model.Name = LecturerToUpdate.Name;
                model.Email = LecturerToUpdate.Email;
                model.DepartmentName = (await _context.Departments.FindAsync(LecturerToUpdate.DepartmentId))?.Name;
                return View(model);
            }
        }
        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.Id == id);
        }
    }
}
