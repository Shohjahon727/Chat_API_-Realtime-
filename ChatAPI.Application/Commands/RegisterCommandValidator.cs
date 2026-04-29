using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
	{
		public RegisterCommandValidator()
		{
			RuleFor(x => x.UserName)
				.NotEmpty()
				.WithMessage("Username is required")
				.MinimumLength(3)
				.WithMessage("Username must be at least 3 characters")
				.MaximumLength(50)
				.WithMessage("Username cannot exceed 50 characters");

			RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage("Email is required")
				.EmailAddress()
				.WithMessage("Invalid email format");

			RuleFor(x => x.Password)
				.NotEmpty()
				.WithMessage("Password is required")
				.MinimumLength(6)
				.WithMessage("Password must be at least 6 characters")
				.MaximumLength(100)
				.WithMessage("Password cannot exceed 100 characters");
		}
	}
}
