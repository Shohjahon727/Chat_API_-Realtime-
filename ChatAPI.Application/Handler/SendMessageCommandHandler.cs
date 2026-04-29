using ChatAPI.Application.Commands;
using ChatAPI.Application.Interfaces;
using ChatAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Handler
{
	public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, int>
	{
		private readonly IUnitOfWork _unitOfWork;

		public SendMessageCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<int> Handle(SendMessageCommand request, CancellationToken cancellationToken)
		{
			var message = new Message
			{
				Text = request.Text,
				SenderId = request.SenderId,
				ChatRoomId = request.ChatRoomId,
				CreatedAt = DateTime.UtcNow
			};
			await _unitOfWork.Messages.AddAsync(message);
			await _unitOfWork.SaveChangesAsync();

			return message.Id;
		}
	}
}
