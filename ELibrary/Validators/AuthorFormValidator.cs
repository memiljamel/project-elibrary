using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class AuthorFormValidator : AbstractValidator<AuthorFormViewModel>
    {
        public AuthorFormValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .When(x => x.ID != Guid.Empty);
            
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(100);
        }
    }
}