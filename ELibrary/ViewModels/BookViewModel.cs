using System.ComponentModel.DataAnnotations;
using ELibrary.Enums;

namespace ELibrary.ViewModels
{
    public class BookViewModel
    {
        public Guid ID { get; set; }

        public string Title { get; set; }

        [Display(Name = "Authors")]
        public string AuthorNames { get; set; }

        public CategoryEnum Category { get; set; }

        public string Publisher { get; set; }

        [Display(Name = "Qty")]
        public int Quantity { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}
