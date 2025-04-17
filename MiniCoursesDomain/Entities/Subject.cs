using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;
using System.ComponentModel.DataAnnotations;

namespace MiniCoursesDomain.Entities;

public class Subject
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public Semester Semester { get; set; }
    public User Professor { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    public SemesterType SemesterType { get; set; }
    [Required]
    [Range(2020, 2050)]
    public int Year { get; set; }
}