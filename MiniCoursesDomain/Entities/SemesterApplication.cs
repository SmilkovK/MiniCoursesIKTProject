using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;
using System.ComponentModel.DataAnnotations;

namespace MiniCoursesDomain.Entities;

public class SemesterApplication
{
    public Guid Id { get; set; }

    [Required]
    public Semester Semester { get; set; }

    [Required]
    public SemesterType SemesterType { get; set; }

    [Range(2000, 2100)]
    public int Year { get; set; }

    public SubjectRequestStatus Status { get; set; } = SubjectRequestStatus.Pending;

    [Required]
    public string StudentId { get; set; }

    public User? Student { get; set; }
}