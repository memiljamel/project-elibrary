using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class BorrowingCreateValidator : AbstractValidator<BorrowingCreateViewModel>
    {
        public BorrowingCreateValidator()
        {
            RuleFor(x => x.MemberID)
                .NotEmpty()
                .WithName("Member Number");

            RuleFor(x => x.BookID)
                .NotEmpty()
                .WithName("Title");
            
            RuleFor(x => x.DateBorrow)
                .NotEmpty()
                .Equal(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}