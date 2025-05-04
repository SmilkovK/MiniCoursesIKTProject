// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Identity;
using MiniCoursesService.Interface;
using GemBox.Document;
using GemBox.Document.Tables;

namespace MiniCoursesIKTProject.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Indeks { get; set; }
        public string Role { get; set; }
        public List<StudentSubject> SubjectsGrades { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            Username = userName;
            FullName = $"{user.Name} {user.LastName}";
            Email = user.Email;
            Indeks = user.Indeks;
            Role = roles.FirstOrDefault() ?? "Student";
            SubjectsGrades = _userService.GetByIdAsync(user.Id).Result.SubjectsGrades;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }


        public IActionResult OnGetExportStudentGrades()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            LoadAsync(user).Wait();

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "GradesTemplate.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{StudentName}}", FullName);
            document.Content.Replace("{{Indeks}}", Indeks);

            var table = new Table(document);

            var headerRow = new TableRow(document);
            headerRow.Cells.Add(new TableCell(document, "Code"));
            headerRow.Cells.Add(new TableCell(document, "Subject Name"));
            headerRow.Cells.Add(new TableCell(document, "Grade"));
            table.Rows.Add(headerRow);

            foreach (var sg in SubjectsGrades)
            {
                if (sg.Grade.HasValue)
                {
                    var row = new TableRow(document);
                    row.Cells.Add(new TableCell(document, sg.Subject.Code));
                    row.Cells.Add(new TableCell(document, sg.Subject.Name));
                    row.Cells.Add(new TableCell(document, sg.Grade.ToString()));
                    table.Rows.Add(row);
                }
            }
            
            var section = document.Sections[0]; 
            section.Blocks.Add(table);

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), "application/pdf", "Grades.pdf");
        }

    }
}
