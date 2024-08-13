using System.ComponentModel.DataAnnotations;

namespace ELibrary.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}