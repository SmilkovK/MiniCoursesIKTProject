using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Identity;

namespace MiniCoursesRepository;

public class ApplicationDbContext : IdentityDbContext<User>
{

    public virtual DbSet<Subject> Subjects { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}