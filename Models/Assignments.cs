using System.Runtime.CompilerServices;

namespace LearningApp.Models
{
    public partial class Assignments
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
