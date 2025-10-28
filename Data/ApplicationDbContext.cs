using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Transcript> Transcripts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============ Identity seed ============
            string ADMIN_ID = Guid.NewGuid().ToString();
            string ROLE_ADMIN_ID = Guid.NewGuid().ToString();
            string ROLE_GV_ID = Guid.NewGuid().ToString();
            string ROLE_SV_ID = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = ROLE_ADMIN_ID, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = ROLE_GV_ID, Name = "GiangVien", NormalizedName = "GIANGVIEN" },
                new IdentityRole { Id = ROLE_SV_ID, Name = "SinhVien", NormalizedName = "SINHVIEN" }
            );

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@qlsv.com",
                NormalizedEmail = "ADMIN@QLSV.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "1")
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = ROLE_ADMIN_ID,
                    UserId = ADMIN_ID
                }
            );

            //// 🔹 Quan hệ 1–nhiều: Major có nhiều Student
            //modelBuilder.Entity<Student>()
            //    .HasOne(s => s.Major)
            //    .WithMany(m => m.Students)
            //    .HasForeignKey(s => s.MajorCode)
            //    .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Seed dữ liệu mẫu cho Major
            modelBuilder.Entity<Major>().HasData(
                new Major { MajorId = "NCNTT", Name = "Công nghệ thông tin", DepartmentId = "KCNTT", TotalCredits = 120 },
                new Major { MajorCode = "KT", Name = "Kinh tế", Detail = "Tài chính, quản trị", TotalCredits = 130 },
                new Major { MajorCode = "KHMT", Name = "Khoa học máy tính", Detail = "Thuật toán", TotalCredits = 125 },
                new Major { MajorCode = "NNA", Name = "Ngôn ngữ Anh", Detail = "Tiếng Anh và văn hóa", TotalCredits = 115 }
            );

            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentCode = "KCNTT", Name = "Công nghệ thông tin" },
                new Department { DepartmentCode = "KKT", Name = "Kinh tế"},
                new Department { DepartmentCode = "KNN", Name = "Ngoại ngữ"},
                new Department { DepartmentCode = "KKHMT", Name = "Khoa học máy tính"}
            );
        }
    }
}
