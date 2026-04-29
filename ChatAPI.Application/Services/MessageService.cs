using ChatAPI.Application.Commands;
using ChatAPI.Application.DTOs;
using ChatAPI.Application.Interfaces;
using ChatAPI.Application.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Services
{
	public class MessageService : IMessageService
	{
		private readonly IMediator _mediator;

		public MessageService(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<int> SendMessageAsync(int senderId, int chatRoomId, string text)
		{
			var command = new SendMessageCommand
			{
				SenderId = senderId,
				ChatRoomId = chatRoomId,
				Text = text
			};

			return await _mediator.Send(command);
		}

		public async Task<PagedResponseDto<MessageDto>> GetMessagesAsync(int chatRoomId, int pageNumber = 1, int pageSize = 20)
		{
			var query = new GetMessagesQuery
			{
				ChatRoomId = chatRoomId,
				PageNumber = pageNumber,
				PageSize = pageSize
			};

			return await _mediator.Send(query);
		}
	}
}
