using ELibrary.Repositories;
using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class EmployeeEditValidator : AbstractValidator<EmployeeEditViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public EmployeeEditValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            RuleFor(x => x.ID)
                .NotEmpty();
            
            RuleFor(x => x.EmployeeNumber)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(16)
                .Must(BeUniqueEmployeeNumber);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);

            RuleFor(x => x.AccessLevel)
                .NotNull()
                .IsInEnum()
                .WithName("Role");

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(100);

            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Password);
        }

        private bool BeUniqueEmployeeNumber(EmployeeEditViewModel item, string employeeNumber)
        {
            return _unitOfWork.EmployeeRepository.IsEmployeeNumberUnique(employeeNumber, item.ID);
        }
    }
}