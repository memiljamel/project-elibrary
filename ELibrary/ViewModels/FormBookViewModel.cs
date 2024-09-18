using System.ComponentModel.DataAnnotations;
using ELibrary.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class FormBookViewModel
    {
        [HiddenInput]
        public Guid ID { get; set; }

        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Display(Name = "Authors")]
        public IEnumerable<Guid> AuthorIDs { get; set; }

        public CategoryEnum Category { get; set; }

        [DataType(DataType.Text)]
        public string Publisher { get; set; }

        public int Quantity { get; set; }
    }
}
