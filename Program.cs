public class Program
{
    private static readonly SchoolDataStore DataStore = new();

    public static void Main(string[] args)
    {
        DataStore.Load();
        SeedStarterData();
        RunApplication();
    }

    private static void RunApplication()
    {
        var exitRequested = false;

        while (!exitRequested)
        {
            SafeClear();
            ShowDashboard();

            Console.WriteLine("1. Manage students");
            Console.WriteLine("2. Manage lecturers");
            Console.WriteLine("3. Manage courses");
            Console.WriteLine("4. Enroll a student in a course");
            Console.WriteLine("5. Record or update student result");
            Console.WriteLine("6. View reports");
            Console.WriteLine("7. Save data");
            Console.WriteLine("0. Exit");

            switch (Prompt("Choose an option: "))
            {
                case "1":
                    ManageStudents();
                    break;
                case "2":
                    ManageLecturers();
                    break;
                case "3":
                    ManageCourses();
                    break;
                case "4":
                    EnrollStudentInCourse();
                    break;
                case "5":
                    RecordResult();
                    break;
                case "6":
                    ViewReports();
                    break;
                case "7":
                    DataStore.Save();
                    Pause("Data saved successfully.");
                    break;
                case "0":
                    DataStore.Save();
                    exitRequested = true;
                    break;
                default:
                    Pause("Please choose a valid option.");
                    break;
            }
        }
    }

    private static void ShowDashboard()
    {
        Console.WriteLine("School Manager");
        Console.WriteLine("==============");
        Console.WriteLine($"Students   : {DataStore.Students.Count}");
        Console.WriteLine($"Lecturers  : {DataStore.Lecturers.Count}");
        Console.WriteLine($"Courses    : {DataStore.Courses.Count}");
        Console.WriteLine($"Enrollments: {DataStore.Enrollments.Count}");
        Console.WriteLine($"Results    : {DataStore.Results.Count}");
        Console.WriteLine();
    }

    private static void ManageStudents()
    {
        var back = false;
        while (!back)
        {
            SafeClear();
            Console.WriteLine("Student Management");
            Console.WriteLine("==================");
            DisplayStudents();
            Console.WriteLine();
            Console.WriteLine("1. Add student");
            Console.WriteLine("2. View student details");
            Console.WriteLine("0. Back");

            switch (Prompt("Choose an option: "))
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    ViewStudentDetails();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Pause("Please choose a valid option.");
                    break;
            }
        }
    }

    private static void ManageLecturers()
    {
        var back = false;
        while (!back)
        {
            SafeClear();
            Console.WriteLine("Lecturer Management");
            Console.WriteLine("===================");
            DisplayLecturers();
            Console.WriteLine();
            Console.WriteLine("1. Add lecturer");
            Console.WriteLine("2. View lecturer details");
            Console.WriteLine("0. Back");

            switch (Prompt("Choose an option: "))
            {
                case "1":
                    AddLecturer();
                    break;
                case "2":
                    ViewLecturerDetails();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Pause("Please choose a valid option.");
                    break;
            }
        }
    }

    private static void ManageCourses()
    {
        var back = false;
        while (!back)
        {
            SafeClear();
            Console.WriteLine("Course Management");
            Console.WriteLine("=================");
            DisplayCourses();
            Console.WriteLine();
            Console.WriteLine("1. Add course");
            Console.WriteLine("2. Assign lecturer to course");
            Console.WriteLine("3. View course roster");
            Console.WriteLine("0. Back");

            switch (Prompt("Choose an option: "))
            {
                case "1":
                    AddCourse();
                    break;
                case "2":
                    AssignLecturerToCourse();
                    break;
                case "3":
                    ViewCourseRoster();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Pause("Please choose a valid option.");
                    break;
            }
        }
    }

    private static void AddStudent()
    {
        var student = new Student
        {
            Id = NextId(DataStore.Students.Select(x => x.Id)),
            RegistrationNumber = PromptRequired("Registration number: "),
            FirstName = PromptRequired("First name: "),
            LastName = PromptRequired("Last name: "),
            Age = PromptInt("Age: "),
            Gender = PromptRequired("Gender: "),
            ClassLevel = PromptRequired("Class level: "),
            Department = PromptRequired("Department: "),
            Email = PromptRequired("Email: "),
            PhoneNumber = PromptRequired("Phone number: ")
        };

        DataStore.Students.Add(student);
        DataStore.Save();
        Pause($"Student {student.FullName} added successfully.");
    }

    private static void ViewStudentDetails()
    {
        if (!EnsureAnyStudents())
        {
            return;
        }

        var studentId = PromptInt("Enter student id: ");
        var student = DataStore.Students.FirstOrDefault(x => x.Id == studentId);
        if (student is null)
        {
            Pause("Student not found.");
            return;
        }

        SafeClear();
        Console.WriteLine($"Student: {student.FullName}");
        Console.WriteLine($"Registration Number: {student.RegistrationNumber}");
        Console.WriteLine($"Department: {student.Department}");
        Console.WriteLine($"Class Level: {student.ClassLevel}");
        Console.WriteLine($"Email: {student.Email}");
        Console.WriteLine($"Phone: {student.PhoneNumber}");
        Console.WriteLine();

        var enrollments = DataStore.Enrollments.Where(x => x.StudentId == student.Id).ToList();
        Console.WriteLine("Enrolled Courses");
        Console.WriteLine("----------------");
        if (enrollments.Count == 0)
        {
            Console.WriteLine("No course enrollments yet.");
        }
        else
        {
            foreach (var enrollment in enrollments)
            {
                var course = DataStore.Courses.FirstOrDefault(x => x.Id == enrollment.CourseId);
                Console.WriteLine($"{course?.CourseCode ?? "N/A"} - {course?.Name ?? "Unknown"} ({enrollment.Semester} {enrollment.Session})");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"CGPA: {CalculateCgpa(student.Id):0.00}");
        Pause();
    }

    private static void AddLecturer()
    {
        var lecturer = new Lecturer
        {
            Id = NextId(DataStore.Lecturers.Select(x => x.Id)),
            StaffNumber = PromptRequired("Staff number: "),
            FirstName = PromptRequired("First name: "),
            LastName = PromptRequired("Last name: "),
            Department = PromptRequired("Department: "),
            Email = PromptRequired("Email: "),
            PhoneNumber = PromptRequired("Phone number: ")
        };

        DataStore.Lecturers.Add(lecturer);
        DataStore.Save();
        Pause($"Lecturer {lecturer.FullName} added successfully.");
    }

    private static void ViewLecturerDetails()
    {
        if (!EnsureAnyLecturers())
        {
            return;
        }

        var lecturerId = PromptInt("Enter lecturer id: ");
        var lecturer = DataStore.Lecturers.FirstOrDefault(x => x.Id == lecturerId);
        if (lecturer is null)
        {
            Pause("Lecturer not found.");
            return;
        }

        SafeClear();
        Console.WriteLine($"Lecturer: {lecturer.FullName}");
        Console.WriteLine($"Staff Number: {lecturer.StaffNumber}");
        Console.WriteLine($"Department: {lecturer.Department}");
        Console.WriteLine($"Email: {lecturer.Email}");
        Console.WriteLine($"Phone: {lecturer.PhoneNumber}");
        Console.WriteLine();
        Console.WriteLine("Assigned Courses");
        Console.WriteLine("----------------");

        var courses = DataStore.Courses.Where(x => x.LecturerId == lecturer.Id).ToList();
        if (courses.Count == 0)
        {
            Console.WriteLine("No courses assigned yet.");
        }
        else
        {
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Id}. {course.CourseCode} - {course.Name}");
            }
        }

        Pause();
    }

    private static void AddCourse()
    {
        var course = new Course
        {
            Id = NextId(DataStore.Courses.Select(x => x.Id)),
            Name = PromptRequired("Course name: "),
            CourseCode = PromptRequired("Course code: "),
            Description = PromptRequired("Description: "),
            Credits = PromptInt("Credits: "),
            Department = PromptRequired("Department: ")
        };

        DataStore.Courses.Add(course);
        DataStore.Save();
        Pause($"Course {course.CourseCode} added successfully.");
    }

    private static void AssignLecturerToCourse()
    {
        if (!EnsureAnyCourses() || !EnsureAnyLecturers())
        {
            return;
        }

        var courseId = PromptInt("Enter course id: ");
        var course = DataStore.Courses.FirstOrDefault(x => x.Id == courseId);
        if (course is null)
        {
            Pause("Course not found.");
            return;
        }

        var lecturerId = PromptInt("Enter lecturer id: ");
        var lecturer = DataStore.Lecturers.FirstOrDefault(x => x.Id == lecturerId);
        if (lecturer is null)
        {
            Pause("Lecturer not found.");
            return;
        }

        course.LecturerId = lecturer.Id;
        course.Instructor = lecturer.FullName;
        DataStore.Save();
        Pause($"{lecturer.FullName} is now assigned to {course.CourseCode}.");
    }

    private static void ViewCourseRoster()
    {
        if (!EnsureAnyCourses())
        {
            return;
        }

        var courseId = PromptInt("Enter course id: ");
        var course = DataStore.Courses.FirstOrDefault(x => x.Id == courseId);
        if (course is null)
        {
            Pause("Course not found.");
            return;
        }

        SafeClear();
        Console.WriteLine($"Course Roster: {course.CourseCode} - {course.Name}");
        Console.WriteLine("-----------------------------------------");
        var enrollments = DataStore.Enrollments.Where(x => x.CourseId == course.Id).ToList();

        if (enrollments.Count == 0)
        {
            Console.WriteLine("No students enrolled yet.");
        }
        else
        {
            foreach (var enrollment in enrollments)
            {
                var student = DataStore.Students.FirstOrDefault(x => x.Id == enrollment.StudentId);
                Console.WriteLine($"{student?.Id}. {student?.FullName} - {enrollment.Semester} {enrollment.Session}");
            }
        }

        Pause();
    }

    private static void EnrollStudentInCourse()
    {
        if (!EnsureAnyStudents() || !EnsureAnyCourses())
        {
            return;
        }

        DisplayStudents();
        var studentId = PromptInt("Enter student id: ");
        var student = DataStore.Students.FirstOrDefault(x => x.Id == studentId);
        if (student is null)
        {
            Pause("Student not found.");
            return;
        }

        Console.WriteLine();
        DisplayCourses();
        var courseId = PromptInt("Enter course id: ");
        var course = DataStore.Courses.FirstOrDefault(x => x.Id == courseId);
        if (course is null)
        {
            Pause("Course not found.");
            return;
        }

        var semester = PromptRequired("Semester: ");
        var session = PromptRequired("Session: ");

        var alreadyEnrolled = DataStore.Enrollments.Any(x =>
            x.StudentId == studentId &&
            x.CourseId == courseId &&
            x.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase) &&
            x.Session.Equals(session, StringComparison.OrdinalIgnoreCase));

        if (alreadyEnrolled)
        {
            Pause("That student is already enrolled in this course for the selected term.");
            return;
        }

        DataStore.Enrollments.Add(new Enrollment
        {
            Id = NextId(DataStore.Enrollments.Select(x => x.Id)),
            StudentId = studentId,
            CourseId = courseId,
            Semester = semester,
            Session = session,
            EnrolledAt = DateTime.UtcNow
        });

        DataStore.Save();
        Pause($"{student.FullName} enrolled in {course.CourseCode} successfully.");
    }

    private static void RecordResult()
    {
        if (!EnsureAnyEnrollments())
        {
            return;
        }

        SafeClear();
        Console.WriteLine("Record Student Result");
        Console.WriteLine("=====================");
        DisplayEnrollments();

        var studentId = PromptInt("Enter student id: ");
        var courseId = PromptInt("Enter course id: ");
        var enrollment = DataStore.Enrollments.FirstOrDefault(x => x.StudentId == studentId && x.CourseId == courseId);
        if (enrollment is null)
        {
            Pause("No enrollment found for that student and course.");
            return;
        }

        var score = PromptDouble("Score (0-100): ", 0, 100);
        var grade = CalculateGrade(score);
        var existingResult = DataStore.Results.FirstOrDefault(x =>
            x.StudentId == studentId &&
            x.CourseId == courseId &&
            x.Semester.Equals(enrollment.Semester, StringComparison.OrdinalIgnoreCase) &&
            x.Session.Equals(enrollment.Session, StringComparison.OrdinalIgnoreCase));

        if (existingResult is null)
        {
            DataStore.Results.Add(new Result
            {
                Id = NextId(DataStore.Results.Select(x => x.Id)),
                StudentId = studentId,
                CourseId = courseId,
                Score = score,
                Grade = grade,
                Semester = enrollment.Semester,
                Session = enrollment.Session
            });
        }
        else
        {
            existingResult.Score = score;
            existingResult.Grade = grade;
        }

        DataStore.Save();
        Pause($"Result saved with grade {grade}.");
    }

    private static void ViewReports()
    {
        var back = false;
        while (!back)
        {
            SafeClear();
            Console.WriteLine("Reports");
            Console.WriteLine("=======");
            Console.WriteLine("1. Student transcript");
            Console.WriteLine("2. Course performance summary");
            Console.WriteLine("3. Top performing students");
            Console.WriteLine("0. Back");

            switch (Prompt("Choose an option: "))
            {
                case "1":
                    ShowStudentTranscript();
                    break;
                case "2":
                    ShowCoursePerformanceSummary();
                    break;
                case "3":
                    ShowTopPerformingStudents();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Pause("Please choose a valid option.");
                    break;
            }
        }
    }

    private static void ShowStudentTranscript()
    {
        if (!EnsureAnyStudents())
        {
            return;
        }

        var studentId = PromptInt("Enter student id: ");
        var student = DataStore.Students.FirstOrDefault(x => x.Id == studentId);
        if (student is null)
        {
            Pause("Student not found.");
            return;
        }

        SafeClear();
        Console.WriteLine($"Transcript: {student.FullName}");
        Console.WriteLine("==============================");
        var results = DataStore.Results.Where(x => x.StudentId == studentId).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No results recorded yet.");
        }
        else
        {
            foreach (var result in results)
            {
                var course = DataStore.Courses.FirstOrDefault(x => x.Id == result.CourseId);
                Console.WriteLine($"{course?.CourseCode ?? "N/A"} - {course?.Name ?? "Unknown"} | Score: {result.Score:0.0} | Grade: {result.Grade} | {result.Semester} {result.Session}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"CGPA: {CalculateCgpa(studentId):0.00}");
        Pause();
    }

    private static void ShowCoursePerformanceSummary()
    {
        if (!EnsureAnyCourses())
        {
            return;
        }

        var courseId = PromptInt("Enter course id: ");
        var course = DataStore.Courses.FirstOrDefault(x => x.Id == courseId);
        if (course is null)
        {
            Pause("Course not found.");
            return;
        }

        SafeClear();
        Console.WriteLine($"Performance Summary: {course.CourseCode} - {course.Name}");
        Console.WriteLine("==============================================");
        var results = DataStore.Results.Where(x => x.CourseId == courseId).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No results recorded for this course.");
            Pause();
            return;
        }

        var average = results.Average(x => x.Score);
        Console.WriteLine($"Students graded: {results.Count}");
        Console.WriteLine($"Average score : {average:0.00}");
        Console.WriteLine();

        foreach (var result in results.OrderByDescending(x => x.Score))
        {
            var student = DataStore.Students.FirstOrDefault(x => x.Id == result.StudentId);
            Console.WriteLine($"{student?.FullName ?? "Unknown"} | Score: {result.Score:0.0} | Grade: {result.Grade}");
        }

        Pause();
    }

    private static void ShowTopPerformingStudents()
    {
        SafeClear();
        Console.WriteLine("Top Performing Students");
        Console.WriteLine("=======================");

        var rankedStudents = DataStore.Students
            .Select(student => new
            {
                Student = student,
                Cgpa = CalculateCgpa(student.Id)
            })
            .Where(x => x.Cgpa > 0)
            .OrderByDescending(x => x.Cgpa)
            .Take(10)
            .ToList();

        if (rankedStudents.Count == 0)
        {
            Console.WriteLine("No graded students available yet.");
        }
        else
        {
            foreach (var item in rankedStudents)
            {
                Console.WriteLine($"{item.Student.FullName} ({item.Student.RegistrationNumber}) - CGPA {item.Cgpa:0.00}");
            }
        }

        Pause();
    }

    private static void DisplayStudents()
    {
        if (DataStore.Students.Count == 0)
        {
            Console.WriteLine("No students added yet.");
            return;
        }

        foreach (var student in DataStore.Students.OrderBy(x => x.Id))
        {
            Console.WriteLine($"{student.Id}. {student.FullName} - {student.RegistrationNumber} - {student.Department}");
        }
    }

    private static void DisplayLecturers()
    {
        if (DataStore.Lecturers.Count == 0)
        {
            Console.WriteLine("No lecturers added yet.");
            return;
        }

        foreach (var lecturer in DataStore.Lecturers.OrderBy(x => x.Id))
        {
            Console.WriteLine($"{lecturer.Id}. {lecturer.FullName} - {lecturer.StaffNumber} - {lecturer.Department}");
        }
    }

    private static void DisplayCourses()
    {
        if (DataStore.Courses.Count == 0)
        {
            Console.WriteLine("No courses added yet.");
            return;
        }

        foreach (var course in DataStore.Courses.OrderBy(x => x.Id))
        {
            var instructor = ResolveInstructorName(course);
            Console.WriteLine($"{course.Id}. {course.CourseCode} - {course.Name} ({course.Department}) | Lecturer: {instructor}");
        }
    }

    private static void DisplayEnrollments()
    {
        foreach (var enrollment in DataStore.Enrollments.OrderBy(x => x.Id))
        {
            var student = DataStore.Students.FirstOrDefault(x => x.Id == enrollment.StudentId);
            var course = DataStore.Courses.FirstOrDefault(x => x.Id == enrollment.CourseId);
            Console.WriteLine($"{student?.Id}. {student?.FullName} | {course?.Id}. {course?.CourseCode} - {course?.Name} | {enrollment.Semester} {enrollment.Session}");
        }
    }

    private static string ResolveInstructorName(Course course)
    {
        if (course.LecturerId is not null)
        {
            var lecturer = DataStore.Lecturers.FirstOrDefault(x => x.Id == course.LecturerId.Value);
            if (lecturer is not null)
            {
                return lecturer.FullName;
            }
        }

        return string.IsNullOrWhiteSpace(course.Instructor) ? "Not assigned" : course.Instructor;
    }

    private static double CalculateCgpa(int studentId)
    {
        var results = DataStore.Results.Where(x => x.StudentId == studentId).ToList();
        if (results.Count == 0)
        {
            return 0;
        }

        double totalQualityPoints = 0;
        int totalCredits = 0;

        foreach (var result in results)
        {
            var course = DataStore.Courses.FirstOrDefault(x => x.Id == result.CourseId);
            if (course is null)
            {
                continue;
            }

            totalQualityPoints += GradeToPoint(result.Grade) * course.Credits;
            totalCredits += course.Credits;
        }

        return totalCredits == 0 ? 0 : totalQualityPoints / totalCredits;
    }

    private static double GradeToPoint(string grade)
    {
        return grade.ToUpperInvariant() switch
        {
            "A" => 5.0,
            "B" => 4.0,
            "C" => 3.0,
            "D" => 2.0,
            "E" => 1.0,
            _ => 0.0
        };
    }

    private static string CalculateGrade(double score)
    {
        if (score >= 70) return "A";
        if (score >= 60) return "B";
        if (score >= 50) return "C";
        if (score >= 45) return "D";
        if (score >= 40) return "E";
        return "F";
    }

    private static int NextId(IEnumerable<int> ids)
    {
        return ids.Any() ? ids.Max() + 1 : 1;
    }

    private static string Prompt(string label)
    {
        Console.Write(label);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    private static string PromptRequired(string label)
    {
        while (true)
        {
            var value = Prompt(label);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Console.WriteLine("This field is required.");
        }
    }

    private static int PromptInt(string label)
    {
        while (true)
        {
            if (int.TryParse(Prompt(label), out var number))
            {
                return number;
            }

            Console.WriteLine("Please enter a valid number.");
        }
    }

    private static double PromptDouble(string label, double min, double max)
    {
        while (true)
        {
            if (double.TryParse(Prompt(label), out var number) && number >= min && number <= max)
            {
                return number;
            }

            Console.WriteLine($"Please enter a number between {min} and {max}.");
        }
    }

    private static bool EnsureAnyStudents()
    {
        if (DataStore.Students.Count > 0)
        {
            return true;
        }

        Pause("No students available yet.");
        return false;
    }

    private static bool EnsureAnyLecturers()
    {
        if (DataStore.Lecturers.Count > 0)
        {
            return true;
        }

        Pause("No lecturers available yet.");
        return false;
    }

    private static bool EnsureAnyCourses()
    {
        if (DataStore.Courses.Count > 0)
        {
            return true;
        }

        Pause("No courses available yet.");
        return false;
    }

    private static bool EnsureAnyEnrollments()
    {
        if (DataStore.Enrollments.Count > 0)
        {
            return true;
        }

        Pause("No enrollments available yet.");
        return false;
    }

    private static void Pause(string? message = null)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine();
            Console.WriteLine(message);
        }

        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }

    private static void SafeClear()
    {
        if (Console.IsOutputRedirected)
        {
            return;
        }

        Console.Clear();
    }

    private static void SeedStarterData()
    {
        var changed = false;

        if (DataStore.Courses.Count == 0)
        {
            DataStore.Courses.AddRange(new[]
            {
                new Course
                {
                    Id = 1,
                    Name = "Mathematics",
                    CourseCode = "MTH101",
                    Instructor = "Dr. Smith",
                    Description = "Basic mathematics course",
                    Credits = 3,
                    Department = "Science"
                },
                new Course
                {
                    Id = 2,
                    Name = "Physics",
                    CourseCode = "PHY102",
                    Instructor = "Dr. Johnson",
                    Description = "Introductory physics course",
                    Credits = 4,
                    Department = "Science"
                }
            });
            changed = true;
        }

        if (changed)
        {
            DataStore.Save();
        }
    }
}
