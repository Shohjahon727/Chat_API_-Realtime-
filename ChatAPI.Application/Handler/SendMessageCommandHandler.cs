using ChatAPI.Application.Commands;
using ChatAPI.Application.Interfaces;
using ChatAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
			var chatRoom = await _unitOfWork.ChatRooms.GetByIdAsync(request.ChatRoomId, cancellationToken);
			if (chatRoom == null)
				throw new InvalidOperationException($"ChatRoom with ID {request.ChatRoomId} not found");

			var user = await _unitOfWork.Users.GetByIdAsync(request.SenderId, cancellationToken);
			if (user == null)
				throw new InvalidOperationException($"User with ID {request.SenderId} not found");

			var userChat = await _unitOfWork.UserChats.Query()
				.FirstOrDefaultAsync(uc => uc.UserId == request.SenderId && uc.ChatRoomId == request.ChatRoomId, cancellationToken);
			if (userChat == null)
				throw new InvalidOperationException($"User {request.SenderId} is not a member of ChatRoom {request.ChatRoomId}");

			var message = new Message
			{
				Text = request.Text.Trim(),
				SenderId = request.SenderId,
				ChatRoomId = request.ChatRoomId,
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWork.Messages.AddAsync(message, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return message.Id;
		}
	}
}
