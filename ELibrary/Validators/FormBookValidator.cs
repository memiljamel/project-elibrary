using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class FormBookValidator : AbstractValidator<FormBookViewModel>
    {
        public FormBookValidator()
        {
            RuleFor(x => x.ID).NotEmpty().When(x => x.ID != Guid.Empty);

            RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(100);

            RuleFor(x => x.AuthorIDs).NotEmpty().WithName("Authors");

            RuleFor(x => x.Category).NotNull().IsInEnum();

            RuleFor(x => x.Publisher).NotEmpty().MinimumLength(3).MaximumLength(100);

            RuleFor(x => x.Quantity).NotEmpty().InclusiveBetween(1, 100);
        }
    }
}
