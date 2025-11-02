using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Models;

namespace QuanLySinhVien_BTL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ===================== BẢNG CHÍNH =====================
        public DbSet<Department> Departments { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Transcript> Transcripts { get; set; }

        // ===================== CẤU HÌNH QUAN HỆ =====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ====== CẤU HÌNH QUAN HỆ ======
            // KHOA - NGÀNH: 1 - nhiều
            modelBuilder.Entity<Major>()
                .HasOne(m => m.Department)
                .WithMany(d => d.Majors)
                .HasForeignKey(m => m.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // NGÀNH - SINHVIÊN: 1 - nhiều
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Major)
                .WithMany(m => m.Students)
                .HasForeignKey(s => s.MajorId)
                .OnDelete(DeleteBehavior.Cascade);

            // GIẢNG VIÊN - KHOA: 1 - nhiều
            modelBuilder.Entity<Lecturer>()
                .HasOne(gv => gv.Department)
                .WithMany(d => d.Lecturers)
                .HasForeignKey(gv => gv.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // LỚP HỌC PHẦN - GIẢNG VIÊN / NGÀNH
            modelBuilder.Entity<Course>()
                .HasOne(l => l.Lecturer)
                .WithMany(gv => gv.Courses)
                .HasForeignKey(l => l.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(l => l.Major)
                .WithMany(m => m.Courses)
                .HasForeignKey(l => l.MajorId)
                .OnDelete(DeleteBehavior.Cascade);

            // BẢNG ĐIỂM - SINHVIÊN / LHP
            modelBuilder.Entity<Transcript>()
                .HasOne(bd => bd.Student)
                .WithMany(s => s.Transcripts)
                .HasForeignKey(bd => bd.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transcript>()
                .HasOne(bd => bd.Course)
                .WithMany(l => l.Transcripts)
                .HasForeignKey(bd => bd.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===================== SEED DỮ LIỆU =====================

            // --- Departments ---
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = "KCNTT", Name = "Công nghệ thông tin" },
                new Department { DepartmentId = "KKT", Name = "Kinh tế" },
                new Department { DepartmentId = "KNN", Name = "Ngoại ngữ" },
                new Department { DepartmentId = "KCT", Name = "Công trình" }
            );

            // --- Majors ---
            modelBuilder.Entity<Major>().HasData(
                new Major { MajorId = "CNPM", Name = "Công nghệ phần mềm", DepartmentId = "KCNTT" },
                new Major { MajorId = "HTTT", Name = "Hệ thống thông tin", DepartmentId = "KCNTT"},
                new Major { MajorId = "QTKD", Name = "Quản trị kinh doanh", DepartmentId = "KKT" },
                new Major { MajorId = "NNA", Name = "Ngôn ngữ Anh", DepartmentId = "KNN" }
            );
        }
    }
}
