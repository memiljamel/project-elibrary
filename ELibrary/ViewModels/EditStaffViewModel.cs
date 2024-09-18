using System.ComponentModel.DataAnnotations;
using ELibrary.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class EditStaffViewModel
    {
        [HiddenInput]
        public Guid ID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }

        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Role")]
        public AccessLevelEnum AccessLevel { get; set; }

        [DataType(DataType.Text)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password (optional)")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation (optional)")]
        public string? PasswordConfirmation { get; set; }
    }
}
