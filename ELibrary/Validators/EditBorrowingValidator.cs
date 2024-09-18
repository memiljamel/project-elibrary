using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class EditBorrowingValidator : AbstractValidator<EditBorrowingViewModel>
    {
        public EditBorrowingValidator()
        {
            RuleFor(x => x.ID).NotEmpty();

            RuleFor(x => x.MemberID).NotEmpty().WithName("Member Number");

            RuleFor(x => x.BookID).NotEmpty().WithName("Title");

            RuleFor(x => x.DateBorrow).NotEmpty();

            RuleFor(x => x.DateReturn).GreaterThanOrEqualTo(x => x.DateBorrow);
        }
    }
}
