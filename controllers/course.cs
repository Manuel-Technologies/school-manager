public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Instructor { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int? LecturerId { get; set; }
    public string Department { get; set; } = string.Empty;
}
