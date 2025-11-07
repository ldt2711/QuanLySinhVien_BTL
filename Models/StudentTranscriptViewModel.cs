namespace QuanLySinhVien_BTL.ViewModels
{
    public class StudentTranscriptViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public double ProcessGrade { get; set; }
        public double FinalGrade { get; set; }
        public double GPA { get; set; }
    }
}
