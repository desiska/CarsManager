using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Models.UserViewModels
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6,
            ErrorMessage = "Username cannot be shorter than 6 characters and longer than 50.")]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10,
            ErrorMessage = "Person PIN must be 10 characters long")]
        public string PersonPIN { get; set; }

        [StringLength(10, MinimumLength = 10,
            ErrorMessage = "Telephone number must be 10 characters long")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Roles { get; set; }

        public string RoleId { get; set; }
    }
}
