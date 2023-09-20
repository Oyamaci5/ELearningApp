namespace elearningapp.Models
{

    public class CourseDetailsViewModel
    {
        public string? CourseTitle { get; set; }
        public string? CourseDescription { get; set; }
        public string? CourseCategory { get; set; }
        public string? CourseImageUrl { get; set; }
        public List<Assignments> Assignments { get; set; }
    }
}