using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
	{
		public DeleteUserCommandValidator()
		{
			RuleFor(x => x.UserId)
				.GreaterThan(0)
				.WithMessage("User ID must be greater than 0");
		}
	}
}
