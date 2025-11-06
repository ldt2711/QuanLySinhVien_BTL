using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Data;
using System.Security.Claims;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Sinh Viên")]
    public class StudentProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentProfileController(ApplicationDbContext context)
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
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == CurrentUserId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id, Phone, Address")] Models.Student student)
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

            if (student.Id != StudentToUpdate.Id)
            {
                return Forbid();
            }

            StudentToUpdate.Phone = student.Phone;
            StudentToUpdate.Address = student.Address;

            if (!ModelState.IsValid)
            {
                var StudentWithMajor = await _context.Students.Include(s => s.Major).FirstOrDefaultAsync(s => s.UserId == CurrentUserId);

                if (StudentWithMajor != null)
                {
                    StudentWithMajor.Phone = student.Phone;
                    StudentWithMajor.Address = student.Address;
                }

                return View(StudentWithMajor);
            }
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
        }
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
