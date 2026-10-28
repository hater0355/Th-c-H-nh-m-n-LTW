using Microsoft.EntityFrameworkCore;
using Thực_Hành_Web.Models;

namespace Thực_Hành_Web.Data;

public class SchoolContext(DbContextOptions<SchoolContext> options) : DbContext(options)
{
    public DbSet<Major> Majors => Set<Major>();
    public DbSet<Learner> Learners => Set<Learner>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Major>().ToTable(nameof(Major));
        modelBuilder.Entity<Course>().ToTable(nameof(Course));
        modelBuilder.Entity<Learner>().ToTable(nameof(Learner));
        modelBuilder.Entity<Enrollment>().ToTable(nameof(Enrollment));

        modelBuilder.Entity<Learner>()
            .HasOne(learner => learner.Major)
            .WithMany(major => major.Learners)
            .HasForeignKey(learner => learner.MajorID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enrollment>()
            .HasOne(enrollment => enrollment.Learner)
            .WithMany(learner => learner.Enrollments)
            .HasForeignKey(enrollment => enrollment.LearnerID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enrollment>()
            .HasOne(enrollment => enrollment.Course)
            .WithMany(course => course.Enrollments)
            .HasForeignKey(enrollment => enrollment.CourseID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
