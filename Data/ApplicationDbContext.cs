using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Quan hệ 1–nhiều: Major có nhiều Student
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Major)
                .WithMany(m => m.Students)
                .HasForeignKey(s => s.MajorCode)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Seed dữ liệu mẫu cho Major
            modelBuilder.Entity<Major>().HasData(
                new Major { MajorCode = "CNTT", Name = "Công nghệ thông tin", Detail = "Phần mềm, mạng", TotalCredits = 120 },
                new Major { MajorCode = "KT", Name = "Kinh tế", Detail = "Tài chính, quản trị", TotalCredits = 130 },
                new Major { MajorCode = "KHMT", Name = "Khoa học máy tính", Detail = "Thuật toán", TotalCredits = 125 },
                new Major { MajorCode = "NNA", Name = "Ngôn ngữ Anh", Detail = "Tiếng Anh và văn hóa", TotalCredits = 115 }
            );

            modelBuilder.Entity<Department>().HasData(
                new Department { DeparmentCode = "KCNTT", Name = "Công nghệ thông tin" },
                new Department { DeparmentCode = "KKT", Name = "Kinh tế"},
                new Department { DeparmentCode = "KNN", Name = "Ngoại ngữ"},
                new Department { DeparmentCode = "KKHMT", Name = "Khoa học máy tính"}
            );
        }
    }
}
