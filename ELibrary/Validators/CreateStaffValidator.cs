using ELibrary.Repositories;
using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class CreateStaffValidator : AbstractValidator<CreateStaffViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStaffValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.StaffNumber)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(16)
                .Must(BeUniqueStaffNumber);

            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);

            RuleFor(x => x.AccessLevel).NotNull().IsInEnum().WithName("Role");

            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100)
                .Must(BeUniqueUsername);

            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(100);

            RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password);
        }

        private bool BeUniqueStaffNumber(CreateStaffViewModel item, string staffNumber)
        {
            return _unitOfWork.StaffRepository.IsStaffNumberUnique(staffNumber, null);
        }

        private bool BeUniqueUsername(CreateStaffViewModel item, string username)
        {
            return _unitOfWork.StaffRepository.IsUsernameUnique(username, null);
        }
    }
}
