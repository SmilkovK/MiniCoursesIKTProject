using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace MiniCoursesDomain.DTO
{
    public class UserCreateDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "Indeks is required for students.")]
        public string? Indeks { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and contain at least one letter, one number, and one special character.")]
        public string? Password { get; set; }
    }
}
