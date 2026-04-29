using ChatAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Queries
{
	public class GetMessagesQuery : IRequest<PagedResponseDto<MessageDto>>
	{
		public int ChatRoomId { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 20;
	}
}
