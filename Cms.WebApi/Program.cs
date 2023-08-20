using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CmsDatabaseContext>(options => 
  options.UseInMemoryDatabase("CmsDatabase"));
builder.Services.AddAutoMapper(typeof(CmsMapper));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/courses", async (CmsDatabaseContext db) => 
{
    var result = await db.Courses.ToListAsync();
    return Results.Ok(result);
});

app.MapPost("/courses", async (Course course, CmsDatabaseContext db) =>
{
    db.Courses.Add(course);
    await db.SaveChangesAsync();

    return Results.Created($"/courses/{course.CourseId}", course);
});

app.Run();


public class CmsMapper: Profile
{
    public CmsMapper()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<CourseDto, Course>();
    }
}

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int CourseDuration { get; set; }
    public int CourseType { get; set; }
}


public class CourseDto
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public COURSE_TYPE CourseType { get; set; }
}

public enum COURSE_TYPE
{
    ENGINEERING = 1,
    MEDICAL,
    MANAGMENT
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
