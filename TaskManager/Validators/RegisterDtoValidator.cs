﻿using FluentValidation;
using TaskManager.DTOs;

namespace TaskManager.Validators
{
	public class RegisterDtoValidator : AbstractValidator<RegisterDto>
	{
		public RegisterDtoValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Name is required.")
				.MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email address is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
				.Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
				.Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
				.Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
				.Matches(@"[\!\?\*\.]+").WithMessage("Password must contain at least one (!? *.).");
		}
	}
}