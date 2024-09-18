using System.ComponentModel.DataAnnotations;

namespace ELibrary.ViewModels
{
    public class AuthorViewModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        [DisplayFormat(NullDisplayText = "No Email")]
        public string? Email { get; set; }

        [Display(Name = "Total Books")]
        public int BookCount { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}
