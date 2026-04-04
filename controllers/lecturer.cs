public class Lecturer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Department { get; set; }
    public string? coursesTaught { get; set; }

    public Lecturer(int id, string? name, string? department, string? coursesTaught)
    {
        Id = id;
        Name = name;
        Department = department;
        this.coursesTaught = coursesTaught;
    }

    public void AddLecturer()
    {
        Console.WriteLine("input the name of the lecturer");
        string? lecturerName = Console.ReadLine();
        Console.WriteLine("input the department of the lecturer");
        string? lecturerDepartment = Console.ReadLine();
        Console.WriteLine("input the courses taught by the lecturer");
        string? lecturerCourses = Console.ReadLine();

        this.Name = lecturerName;
        this.Department = lecturerDepartment;
        this.coursesTaught = lecturerCourses;

    }
}