namespace LearningApp.Models
{
    public partial class Courses
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int InstructorId { get; set; }
        public string Category { get; set; }
        public int EnrollmentCount { get; set; }
        public string? ImageUrl { get; set; }

    }
}
