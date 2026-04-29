using ChatAPI.Application.DTOs;
using ChatAPI.Application.Queries;

namespace ChatAPI.Application.Services
{
	public interface IMessageService
	{
		Task<int> SendMessageAsync(int senderId, int chatRoomId, string text);
		Task<PagedResponseDto<MessageDto>> GetMessagesAsync(int chatRoomId, int pageNumber = 1, int pageSize = 20);
	}
}
