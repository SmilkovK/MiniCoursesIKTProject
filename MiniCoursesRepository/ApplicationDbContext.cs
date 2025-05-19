using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Identity;

namespace MiniCoursesRepository;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Subject>()
            .ToTable("Subject")
            .HasOne(s => s.Professor)
            .WithMany()
            .HasForeignKey(s => s.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SemesterApplication>()
            .HasMany(sa => sa.Subjects)
            .WithOne()
            .HasForeignKey("SemesterApplicationId");

        modelBuilder.Entity<Homework>()
            .HasOne(h => h.Subject)
            .WithMany(s => s.Homework)
            .HasForeignKey(h => h.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Homework>()
            .HasOne(h => h.CreatedBy)
            .WithMany()
            .HasForeignKey(h => h.CreatedById)
            .OnDelete(DeleteBehavior.Restrict); // No cascade
    }

    public DbSet<GradedFile> GradedFiles { get; set; }
    public DbSet<Homework> Homeworks { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SemesterApplication> SemesterApplications { get; set; }
}