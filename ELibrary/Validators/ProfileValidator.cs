﻿using ELibrary.Repositories;
using ELibrary.ViewModels;
using FluentValidation;

namespace ELibrary.Validators
{
    public class ProfileValidator : AbstractValidator<ProfileViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.ID).NotEmpty();

            RuleFor(x => x.StaffNumber)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(16)
                .Must(BeUniqueStaffNumber);

            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);

            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100)
                .Must(BeUniqueUsername);

            RuleFor(x => x.Password).MinimumLength(8).MaximumLength(100);

            RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password);
        }

        private bool BeUniqueStaffNumber(ProfileViewModel item, string staffNumber)
        {
            return _unitOfWork.StaffRepository.IsStaffNumberUnique(staffNumber, item.ID);
        }

        private bool BeUniqueUsername(ProfileViewModel item, string username)
        {
            return _unitOfWork.StaffRepository.IsUsernameUnique(username, item.ID);
        }
    }
}
