using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CmsDatabaseContext>(options => 
  options.UseInMemoryDatabase("CmsDatabase"));


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/courses", (CmsDatabaseContext db) => 
{
    var result = db.Courses.ToList();
    return Results.Ok(result);
});

app.Run();

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int CourseDuration { get; set; }
    public int CourseType { get; set; }
}

public class CmsDatabaseContext: DbContext
{
    // It's working with that too
    //public DbSet<Course> Courses { get; set; }

    //Make this to avoid the null, this is a default Set
    public DbSet<Course> Courses => Set<Course>();
    public CmsDatabaseContext(DbContextOptions options) : base(options)
    {
        
    }
}
