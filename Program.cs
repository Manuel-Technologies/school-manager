using System;
using System.IO;
using System.Text.Json;


public class Program
{
  
  public static void Main(string[] args)
    {
        // Create sample data
        var courses = new[]
        {
            new Course { Id = 1, Name = "Mathematics", Instructor = "Dr. Smith", courseCode = 101, Description = "Basic Math Course", Credits = 3 },
            new Course { Id = 2, Name = "Physics", Instructor = "Dr. Johnson", courseCode = 102, Description = "Basic Physics Course", Credits = 4 }
        };

        var results = new[]
        {
            new Result { Id = 1, StudentId = 1, CourseId = 1, Grade = "A" },
            new Result { Id = 2, StudentId = 1, CourseId = 2, Grade = "B" }
        };

        // Serialize to JSON
        var options = new JsonSerializerOptions { WriteIndented = true };
        string coursesJson = JsonSerializer.Serialize(courses, options);
        string resultsJson = JsonSerializer.Serialize(results, options);

        // Write to files
        File.WriteAllText("courses.json", coursesJson);
        File.WriteAllText("results.json", resultsJson);

        Console.WriteLine("Data has been serialized to JSON files.");
    }
    
}