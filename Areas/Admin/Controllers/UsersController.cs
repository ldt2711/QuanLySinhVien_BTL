using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using QuanLySinhVien_BTL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace QuanLySinhVien_BTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index(string searchString, string role)
        {
            ViewData["Title"] = "Danh sách người dùng";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentRole"] = role;

            // Provide roles for filter dropdown
            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Name)).ToList();

            var usersQuery = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                usersQuery = usersQuery.Where(u => u.UserName.Contains(searchString) || u.Email.Contains(searchString));
            }

            var usersList = usersQuery.ToList();

            var model = new List<UserViewModel>();
            foreach (var u in usersList)
            {
                var roles = await _userManager.GetRolesAsync(u);
                model.Add(new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = roles.FirstOrDefault() ?? string.Empty
                });
            }

            if (!string.IsNullOrEmpty(role))
            {
                model = model.Where(m => m.Role == role).ToList();
            }

            return View(model);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
                ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.Role) && await _roleManager.RoleExistsAsync(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Users/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.FirstOrDefault()
            };
            return View(model);
        }

        // POST: Admin/Users/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var e in updateResult.Errors) ModelState.AddModelError(string.Empty, e.Description);
                ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(model);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }
            if (!string.IsNullOrEmpty(model.Role) && await _roleManager.RoleExistsAsync(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Users/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.FirstOrDefault()
            };
            return View(model);
        }

        // POST: Admin/Users/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}