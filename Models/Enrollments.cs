namespace elearningapp.Models
{
    public class Enrollments
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
