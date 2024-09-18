using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class CreateBorrowingValidator : AbstractValidator<CreateBorrowingViewModel>
    {
        public CreateBorrowingValidator()
        {
            RuleFor(x => x.MemberID).NotEmpty().WithName("Member Number");

            RuleFor(x => x.BookID).NotEmpty().WithName("Title");

            RuleFor(x => x.DateBorrow).NotEmpty().Equal(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}
