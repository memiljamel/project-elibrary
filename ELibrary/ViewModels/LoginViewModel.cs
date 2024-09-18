using System.ComponentModel.DataAnnotations;

namespace ELibrary.ViewModels
{
    public class LoginViewModel
    {
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
