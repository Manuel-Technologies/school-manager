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
        Courses = LoadCourses("courses.json");
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

    private List<Course> LoadCourses(string fileName)
    {
        if (!File.Exists(fileName))
        {
            return new List<Course>();
        }

        var json = File.ReadAllText(fileName);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Course>();
        }

        var document = JsonDocument.Parse(json);
        var courses = new List<Course>();

        foreach (var element in document.RootElement.EnumerateArray())
        {
            var course = new Course
            {
                Id = GetInt(element, "Id"),
                Name = GetString(element, "Name"),
                Instructor = GetNullableString(element, "Instructor"),
                CourseCode = GetCourseCode(element),
                Description = GetString(element, "Description"),
                Credits = GetInt(element, "Credits"),
                LecturerId = GetNullableInt(element, "LecturerId"),
                Department = GetString(element, "Department", "General")
            };

            courses.Add(course);
        }

        return courses;
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

    private static string GetCourseCode(JsonElement element)
    {
        if (TryGetPropertyIgnoreCase(element, "CourseCode", out var courseCodeElement))
        {
            return courseCodeElement.ValueKind switch
            {
                JsonValueKind.Number => courseCodeElement.GetInt32().ToString(),
                JsonValueKind.String => courseCodeElement.GetString() ?? string.Empty,
                _ => string.Empty
            };
        }

        if (TryGetPropertyIgnoreCase(element, "courseCode", out var legacyElement))
        {
            return legacyElement.ValueKind switch
            {
                JsonValueKind.Number => legacyElement.GetInt32().ToString(),
                JsonValueKind.String => legacyElement.GetString() ?? string.Empty,
                _ => string.Empty
            };
        }

        return string.Empty;
    }

    private static string GetString(JsonElement element, string propertyName, string defaultValue = "")
    {
        if (TryGetPropertyIgnoreCase(element, propertyName, out var property) && property.ValueKind == JsonValueKind.String)
        {
            return property.GetString() ?? defaultValue;
        }

        return defaultValue;
    }

    private static string? GetNullableString(JsonElement element, string propertyName)
    {
        if (TryGetPropertyIgnoreCase(element, propertyName, out var property) && property.ValueKind == JsonValueKind.String)
        {
            return property.GetString();
        }

        return null;
    }

    private static int GetInt(JsonElement element, string propertyName, int defaultValue = 0)
    {
        if (TryGetPropertyIgnoreCase(element, propertyName, out var property) && property.ValueKind == JsonValueKind.Number)
        {
            return property.GetInt32();
        }

        return defaultValue;
    }

    private static int? GetNullableInt(JsonElement element, string propertyName)
    {
        if (TryGetPropertyIgnoreCase(element, propertyName, out var property) && property.ValueKind == JsonValueKind.Number)
        {
            return property.GetInt32();
        }

        return null;
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement element, string propertyName, out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }
}
