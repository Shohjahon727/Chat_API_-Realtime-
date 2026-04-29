using ChatAPI.Application.DTOs;
using ChatAPI.Application.Interfaces;
using ChatAPI.Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ChatAPI.Application.Handler
{
	public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, List<MessageDto>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetMessagesQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
		{
			var query = _unitOfWork.Messages
				.Query()
				.Where(m => m.ChatRoomId == request.ChatRoomId)
				.OrderByDescending(m => m.CreatedAt);

			var messages = await query
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(m => new MessageDto
				{
					Id = m.Id,
					Text = m.Text,
					SenderId = m.SenderId,
					CreatedAt = m.CreatedAt
				})
				.ToListAsync();

			return messages;
		}
	}
}
