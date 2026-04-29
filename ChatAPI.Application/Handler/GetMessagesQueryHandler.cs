using ChatAPI.Application.DTOs;
using ChatAPI.Application.Interfaces;
using ChatAPI.Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ChatAPI.Application.Handler
{
	public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, PagedResponseDto<MessageDto>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetMessagesQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<PagedResponseDto<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
		{
			var query = _unitOfWork.Messages
				.Query()
				.Where(m => m.ChatRoomId == request.ChatRoomId && !m.IsDeleted)
				.OrderByDescending(m => m.CreatedAt);

			var totalCount = await query.CountAsync(cancellationToken);

			var messages = await query
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(m => new MessageDto
				{
					Id = m.Id,
					Text = m.Text,
					SenderId = m.SenderId,
					SenderName = m.Sender.UserName,
					CreatedAt = m.CreatedAt
				})
				.ToListAsync(cancellationToken);

			return new PagedResponseDto<MessageDto>
			{
				Data = messages,
				TotalCount = totalCount,
				PageNumber = request.PageNumber,
				PageSize = request.PageSize
			};
		}
	}
}
