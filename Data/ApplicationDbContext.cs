
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien_BTL.Models;
namespace QuanLySinhVien_BTL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<QuanLySinhVien_BTL.Models.Student> Students { get; set; } = default!;
        public DbSet<QuanLySinhVien_BTL.Models.Lecturer> Lecturers { get; set; } = default!;
        public DbSet<QuanLySinhVien_BTL.Models.Course> Courses { get; set; } = default!;
    }
}
