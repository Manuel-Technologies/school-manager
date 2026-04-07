using System.Text.Json;

public class SchoolDataStore
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public List<Student> Students { get; private set; } = new();
    public List<Lecturer> Lecturers { get; private set; } = new();
    public List<Course> Courses { get; private set; } = new();
    public List<Enrollment> Enrollments { get; private set; } = new();
    public List<Result> Results { get; private set; } = new();

    public void Load()
    {
        Students = LoadList<Student>("students.json");
        Lecturers = LoadList<Lecturer>("lecturers.json");
        Courses = LoadList<Course>("courses.json");
        Enrollments = LoadList<Enrollment>("enrollments.json");
        Results = LoadList<Result>("results.json");

        NormalizeCourses();
    }

    public void Save()
    {
        SaveList("students.json", Students);
        SaveList("lecturers.json", Lecturers);
        SaveList("courses.json", Courses);
        SaveList("enrollments.json", Enrollments);
        SaveList("results.json", Results);
    }

    private List<T> LoadList<T>(string fileName)
    {
        if (!File.Exists(fileName))
        {
            return new List<T>();
        }

        var json = File.ReadAllText(fileName);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<T>();
        }

        return JsonSerializer.Deserialize<List<T>>(json, _serializerOptions) ?? new List<T>();
    }

    private void SaveList<T>(string fileName, List<T> items)
    {
        var json = JsonSerializer.Serialize(items, _serializerOptions);
        File.WriteAllText(fileName, json);
    }

    private void NormalizeCourses()
    {
        foreach (var course in Courses)
        {
            if (string.IsNullOrWhiteSpace(course.CourseCode))
            {
                course.CourseCode = $"CRS-{course.Id:000}";
            }

            if (string.IsNullOrWhiteSpace(course.Department))
            {
                course.Department = "General";
            }
        }
    }
}
