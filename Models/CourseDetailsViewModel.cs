namespace elearningapp.Models
{

    public class CourseDetailsViewModel
    {
        public string? CourseTitle { get; set; }
        public string? CourseDescription { get; set; }
        public string? CourseCategory { get; set; }
        public string? CourseImageUrl { get; set; }
        public int? CourseEnrollmentCount { get; set; }
        public int? CourseId { get; set; }
        public int? CourseCDuration { get; set; }
        public List<Assignments> Assignments { get; set; }
    }
}