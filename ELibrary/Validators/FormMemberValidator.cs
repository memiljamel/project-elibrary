using ELibrary.Repositories;
using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class FormMemberValidator : AbstractValidator<FormMemberViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FormMemberValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.ID).NotEmpty().When(x => x.ID != Guid.Empty);

            RuleFor(x => x.MemberNumber)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(16)
                .Must(BeUniqueMemberNumber);

            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);

            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);

            RuleFor(x => x.PhoneNumbers)
                .NotEmpty()
                .Matches(
                    @"^(\+?\d{1,4}?[-.\s]?)?(\(?\d{1,4}?\)?[-.\s]?)?\d{1,4}[-.\s]?\d{1,9}(,\s*(\+?\d{1,4}?[-.\s]?)?(\(?\d{1,4}?\)?[-.\s]?)?\d{1,4}[-.\s]?\d{1,9})*$"
                )
                .MaximumLength(100);

            RuleFor(x => x.Address).NotEmpty().Matches(@"^[\w\s,.\-#]+$").MaximumLength(256);
        }

        private bool BeUniqueMemberNumber(FormMemberViewModel item, string memberNumber)
        {
            return _unitOfWork.MemberRepository.IsMemberNumberUnique(memberNumber, item.ID);
        }
    }
}
