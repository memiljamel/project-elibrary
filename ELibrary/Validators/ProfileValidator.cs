using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class ProfileValidator : AbstractValidator<ProfileViewModel>
    {
        public ProfileValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);
            
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(100);

            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Password);
        }
    }
}