using Microsoft.AspNetCore.Identity;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Data
{
    public static class DbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Giảng Viên", "Sinh Viên" };

            // Tạo các vai trò mặc định nếu chưa có
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Admin mặc định
            var adminUser = await userManager.FindByEmailAsync("admin@qlsv.com");
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@qlsv.com",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, "Admin123!"); // mật khẩu mẫu
                if (result.Succeeded){
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    Console.WriteLine($"Admin user creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // Giảng viên mẫu
            //var gvUser = await userManager.FindByEmailAsync("giangvien@qlsv.com");
            //if (gvUser == null)
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "thinh",
            //        Email = "giangvien@qlsv.com",
            //        EmailConfirmed = true
            //    };
            //    var result = await userManager.CreateAsync(user, "thinh123!");
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(user, "Giảng Viên");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"GiangVien user creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            //    }
            //}

            //// Sinh viên mẫu
            //var svUser = await userManager.FindByEmailAsync("sinhvien@qlsv.com");
            //if (svUser == null)
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "leethe",
            //        Email = "sinhvien@qlsv.com",
            //        EmailConfirmed = true
            //    };
            //    var result = await userManager.CreateAsync(user, "leethe123!");
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(user, "Sinh Viên");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"SinhVien user creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            //    }
            //}
        }
    }
}
