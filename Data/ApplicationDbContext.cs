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
                .HasForeignKey(s => s.MajorId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Seed dữ liệu mẫu cho Major
            modelBuilder.Entity<Major>().HasData(
                new Major { Id = 1, Name = "Công nghệ thông tin", Detail = "Phần mềm, mạng", TotalCredits = 120 },
                new Major { Id = 2, Name = "Kinh tế", Detail = "Tài chính, quản trị", TotalCredits = 130 },
                new Major { Id = 3, Name = "Khoa học máy tính", Detail = "Thuật toán", TotalCredits = 125 },
                new Major { Id = 4, Name = "Ngôn ngữ Anh", Detail = "Tiếng Anh và văn hóa", TotalCredits = 115 }
            );

            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Công nghệ thông tin" },
                new Department { Id = 2, Name = "Kinh tế"},
                new Department { Id = 3, Name = "Ngoại ngữ"},
                new Department { Id = 4, Name = "Khoa học máy tính"}
            );
        }
    }
}
