using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
	{
		public SendMessageCommandValidator()
		{
			RuleFor(x => x.Text)
				.NotEmpty()
				.WithMessage("Message text is required")
				.MaximumLength(5000)
				.WithMessage("Message text cannot exceed 5000 characters")
				.Must(text => !string.IsNullOrWhiteSpace(text))
				.WithMessage("Message text cannot be only whitespace");

			RuleFor(x => x.ChatRoomId)
				.GreaterThan(0)
				.WithMessage("Chat room ID must be greater than 0");

			RuleFor(x => x.SenderId)
				.GreaterThan(0)
				.WithMessage("Sender ID must be greater than 0");
		}
	}
}
