using System.ComponentModel.DataAnnotations;

namespace ELibrary.Enums
{
    public enum CategoryEnum
    {
        Fiction,

        [Display(Name = "Non Fiction")]
        NonFiction,
        Biography,
        Automotive,
        Health,
        Technology,
        Science,
        History,
        Art,
        Business,

        [Display(Name = "Self Help")]
        SelfHelp,
        Comics,
        Novel,
        Poetry,
        Encyclopedia,
    }
}
