using ELibrary.Repositories;
using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public EmployeeCreateValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
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
            
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100)
                .Must(BeUniqueUsername);
                
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100);

            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Password);
        }
        
        private bool BeUniqueEmployeeNumber(EmployeeCreateViewModel item, string employeeNumber)
        {
             return _unitOfWork.EmployeeRepository.IsEmployeeNumberUnique(employeeNumber, null);
        }
        
        private bool BeUniqueUsername(EmployeeCreateViewModel item, string username)
        {
            return _unitOfWork.EmployeeRepository.IsUsernameUnique(username, null);
        }
    }
}