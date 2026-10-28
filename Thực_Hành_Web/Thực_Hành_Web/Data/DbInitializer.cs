using Microsoft.EntityFrameworkCore;
using Thực_Hành_Web.Models;

namespace Thực_Hành_Web.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var context = serviceProvider.GetRequiredService<SchoolContext>();
        await context.Database.EnsureCreatedAsync();

        if (await context.Majors.AnyAsync())
        {
            return;
        }

        var majors = new[]
        {
            new Major { MajorName = "IT" },
            new Major { MajorName = "Economics" },
            new Major { MajorName = "Mathematics" }
        };
        context.Majors.AddRange(majors);
        await context.SaveChangesAsync();

        var learners = new[]
        {
            new Learner { FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = new DateTime(2005, 9, 1), MajorID = majors[0].MajorID },
            new Learner { FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = new DateTime(2002, 9, 1), MajorID = majors[1].MajorID }
        };
        context.Learners.AddRange(learners);
        await context.SaveChangesAsync();

        var courses = new[]
        {
            new Course { CourseID = 1050, Title = "Chemistry", Credits = 3 },
            new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3 },
            new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3 }
        };
        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();

        context.Enrollments.AddRange(
            new Enrollment { LearnerID = learners[0].LearnerID, CourseID = 1050, Grade = 5.5f },
            new Enrollment { LearnerID = learners[0].LearnerID, CourseID = 4022, Grade = 7.5f },
            new Enrollment { LearnerID = learners[1].LearnerID, CourseID = 1050, Grade = 3.5f },
            new Enrollment { LearnerID = learners[1].LearnerID, CourseID = 4041, Grade = 7f });
        await context.SaveChangesAsync();
    }
}
