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
				.MaximumLength(500);

			RuleFor(x => x.ChatRoomId)
				.GreaterThan(0);

			RuleFor(x => x.SenderId)
				.GreaterThan(0);
		}
	}
}
