using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using QuanLySinhVien_BTL.Models;
using QuanLySinhVien_BTL.ViewModels;
using System.Security.Claims;

namespace QuanLySinhVien_BTL.Areas.Student.Controllers
{
    [Area("Student")]
    public class StudentProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudentProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MyProfile()
        {
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (CurrentUserId == null)
            {
                return Unauthorized();
            }

            var student = await _context.Students.Include(s => s.Major).FirstOrDefaultAsync(s => s.UserId == CurrentUserId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId == null)
            {
                return Unauthorized();
            }
            var student = await _context.Students.Include(s => s.Major).FirstOrDefaultAsync(s => s.UserId == CurrentUserId);

            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new StudentEditViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                MajorName = student.Major?.Name, // Lấy tên ngành
                Phone = student.Phone,
                Address = student.Address
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentEditViewModel viewModel)
        {
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (CurrentUserId == null)
            {
                return Unauthorized();
            }

            var StudentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.UserId == CurrentUserId);

            if (StudentToUpdate == null)
            {
                return NotFound();
            }

            if (viewModel.Id != StudentToUpdate.Id)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                viewModel.Name = StudentToUpdate.Name;
                viewModel.Email = StudentToUpdate.Email;
                viewModel.MajorName = (await _context.Majors.FindAsync(StudentToUpdate.MajorId))?.Name;
                return View(viewModel);
            }

            StudentToUpdate.Phone = viewModel.Phone;
            StudentToUpdate.Address = viewModel.Address;

            try 
            {
                _context.Update(StudentToUpdate);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!"; // Thông báo thành công
                return RedirectToAction(nameof(MyProfile));
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!StudentExists(StudentToUpdate.Id))
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
                viewModel.Name = StudentToUpdate.Name;
                viewModel.Email = StudentToUpdate.Email;
                viewModel.MajorName = (await _context.Majors.FindAsync(StudentToUpdate.MajorId))?.Name;
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("MyProfile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
