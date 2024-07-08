﻿using FluentValidation;
using TaskManager.DTOs;

namespace TaskManager.Validators
{
	public class LoginDtoValidator : AbstractValidator<LoginDto>
	{
		public LoginDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email address is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.");
		}
	}
}