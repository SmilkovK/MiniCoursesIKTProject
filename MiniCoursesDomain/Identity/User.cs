using Microsoft.AspNetCore.Identity;
using MiniCoursesDomain.Entities;

namespace MiniCoursesDomain.Identity;

public class User : IdentityUser
{
    public string? Indeks { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public List<StudentSubject> SubjectsGrades { get; set; } = new List<StudentSubject>();
    public List<GradedFile> GradedFiles { get; set; } = new List<GradedFile>();
    public List<SemesterApplication> SemesterApplications { get; set; } = new List<SemesterApplication>();
}