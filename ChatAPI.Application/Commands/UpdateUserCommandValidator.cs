using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
	{
		public UpdateUserCommandValidator()
		{
			RuleFor(x => x.UserId)
				.GreaterThan(0)
				.WithMessage("User ID must be greater than 0");

			RuleFor(x => x.UserName)
				.MinimumLength(3)
				.WithMessage("Username must be at least 3 characters")
				.MaximumLength(50)
				.WithMessage("Username cannot exceed 50 characters")
				.When(x => !string.IsNullOrEmpty(x.UserName));

			RuleFor(x => x.Email)
				.EmailAddress()
				.WithMessage("Invalid email format")
				.When(x => !string.IsNullOrEmpty(x.Email));
		}
	}
}
