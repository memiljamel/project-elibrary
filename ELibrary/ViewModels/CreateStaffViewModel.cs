using System.ComponentModel.DataAnnotations;
using ELibrary.Enums;

namespace ELibrary.ViewModels
{
    public class CreateStaffViewModel
    {
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
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirmation { get; set; }
    }
}
