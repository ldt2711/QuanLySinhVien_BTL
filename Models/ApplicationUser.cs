using Microsoft.AspNetCore.Identity;

namespace QuanLySinhVien_BTL.Models
{
    public class ApplicationUser : IdentityUser
    {
    }
    public static class ApplicationDbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "GiangVien", "SinhVien" };

            // Tạo các vai trò mặc định
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
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
                await userManager.CreateAsync(user, "1");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            // Giảng viên mẫu
            var gvUser = await userManager.FindByEmailAsync("giangvien@qlsv.com");
            if (gvUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "thinh",
                    Email = "giangvien@qlsv.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "1");
                await userManager.AddToRoleAsync(user, "GiangVien");
            }

            // Sinh viên mẫu
            var svUser = await userManager.FindByEmailAsync("sinhvien@qlsv.com");
            if (svUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "leethe",
                    Email = "sinhvien@qlsv.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "1");
                await userManager.AddToRoleAsync(user, "SinhVien");
            }
        }
    }
}
