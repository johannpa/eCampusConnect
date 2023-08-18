var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/courses", () => 
{
    
});

app.Run();


public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int CourseDuration { get; set; }
    public int CourseType { get; set; }
}
