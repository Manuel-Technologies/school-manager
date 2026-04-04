public class Student
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Grade { get; set; }
    public string? coursesEnrolled { get; set; }

    public Student(int id, string? name, int age, string? grade, string? coursesEnrolled)
    {
        Id = id;
        Name = name;
        Age = age;
        Grade = grade;
        this.coursesEnrolled = coursesEnrolled;
    }

    public void AddStudent()
    {
        Console.WriteLine("input the name of the student");
        string? studentName = Console.ReadLine();
        Console.WriteLine("input the age of the student");
        int studentAge = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("input the grade of the student");
        string? studentGrade = Console.ReadLine();
        Console.WriteLine("input the courses enrolled by the student");
        string? studentCourses = Console.ReadLine();

        this.Name = studentName;
        this.Age = studentAge;
        this.Grade = studentGrade;
        this.coursesEnrolled = studentCourses;

    }
}