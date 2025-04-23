using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Enums;
using MiniCoursesRepository.Repository.Interfaces;

namespace MiniCoursesIKTProject.Controllers;

public class SemesterController : Controller
{
    private readonly IStudentRepository _studentRepository;

    public SemesterController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    // GET: Semester/Apply
    [HttpGet]
    public IActionResult Apply()
    {
        var model = new SemesterApplication
        {
            Year = DateTime.Now.Year
        };
        return View(model);
    }

    // POST: Semester/Apply
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(SemesterApplication model)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(studentId))
        {
            ModelState.AddModelError("", "User is not logged in.");
            return View(model);
        }

        model.StudentId = studentId;
        model.Status = SubjectRequestStatus.Pending;

        var user = await _studentRepository.GetByIdAsync(model.StudentId);
        if (user == null)
        {
            ModelState.AddModelError("", "Student not found.");
            return View(model);
        }

        user.SemesterApplications.Add(model);
        await _studentRepository.UpdateAsync(user);

        return RedirectToAction("ApplicationStatus");
    }

    // GET: Semester/ApplicationStatus
    [HttpGet]
    public async Task<IActionResult> ApplicationStatus()
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(studentId))
        {
            return Unauthorized("User is not logged in.");
        }

        var user = await _studentRepository.GetByIdAsync(studentId);
        if (user == null)
        {
            return NotFound();
        }

        var applications = user.SemesterApplications;
        return View(applications);
    }

    // GET: Semester/ManageApplications
    [HttpGet]
    public async Task<IActionResult> ManageApplications()
    {
        var users = await _studentRepository.GetAllAsync();
        var pendingApplications = users
            .SelectMany(u => u.SemesterApplications)
            .Where(a => a.Status == SubjectRequestStatus.Pending)
            .ToList();
        return View(pendingApplications);
    }

    // POST: Semester/AcceptApplication
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptApplication(Guid id)
    {
        var users = await _studentRepository.GetAllAsync();
        var application = users
            .SelectMany(u => u.SemesterApplications)
            .FirstOrDefault(a => a.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        application.Status = SubjectRequestStatus.Accepted;
        var user = users.FirstOrDefault(u => u.SemesterApplications.Contains(application));
        if (user != null)
        {
            await _studentRepository.UpdateAsync(user);
        }

        return RedirectToAction("ManageApplications");
    }

    // POST: Semester/RejectApplication
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectApplication(Guid id)
    {
        var users = await _studentRepository.GetAllAsync();
        var application = users
            .SelectMany(u => u.SemesterApplications)
            .FirstOrDefault(a => a.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        application.Status = SubjectRequestStatus.Rejected;
        var user = users.FirstOrDefault(u => u.SemesterApplications.Contains(application));
        if (user != null)
        {
            await _studentRepository.UpdateAsync(user);
        }

        return RedirectToAction("ManageApplications");
    }
}