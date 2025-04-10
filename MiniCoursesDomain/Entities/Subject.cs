using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;

namespace MiniCoursesDomain.Entities;

public class Subject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Semester Semester { get; set; }
    public User Professor { get; set; }
}