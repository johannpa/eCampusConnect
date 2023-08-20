using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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

//app.MapPost("/courses", async ([FromBody] CourseDto courseDto, [FromServices] CmsDatabaseContext db, [FromServices] IMapper mapper)
app.MapPost("/courses", async ( CourseDto courseDto, CmsDatabaseContext db, IMapper mapper) =>
{
    var newCourse = mapper.Map<Course>(courseDto);

    db.Courses.Add(newCourse);
    await db.SaveChangesAsync();

    var result = mapper.Map<CourseDto>(newCourse);
    return Results.Created($"/courses/{result.CourseId}", result);
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
    public int CourseDuration { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
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
