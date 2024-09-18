using System.ComponentModel.DataAnnotations;
using ELibrary.Enums;

namespace ELibrary.ViewModels
{
    public class StaffViewModel
    {
        public Guid ID { get; set; }

        [Display(Name = "Staff Number")]
        public string StaffNumber { get; set; }

        public string Name { get; set; }

        [Display(Name = "Role")]
        public AccessLevelEnum AccessLevel { get; set; }

        public string Username { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}
