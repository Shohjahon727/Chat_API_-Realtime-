using Chat_API.Realtime;
using ChatAPI.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat_API.Hubs;

public class ChatHub : Hub
{
	private readonly IMediator _mediator;

	public ChatHub(IMediator mediator)
	{
		_mediator = mediator;
	}
	public override async Task OnConnectedAsync()
	{
		var userId = int.Parse(Context.User.FindFirst("id").Value);

		ConnectionManager.Users[userId] = Context.ConnectionId;

		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception exception)
	{
		var user = ConnectionManager.Users
			.FirstOrDefault(x => x.Value == Context.ConnectionId);

		if (user.Key != 0)
			ConnectionManager.Users.TryRemove(user.Key, out _);

		await base.OnDisconnectedAsync(exception);
	}
	public async Task SendMessage(SendMessageCommand command)
	{
		// 1. DB ga yozish
		var messageId = await _mediator.Send(command);

		// 2. realtime yuborish
		await Clients.Group(command.ChatRoomId.ToString()).SendAsync("ReciveMessage", command);
	}

	public async Task JoinRoom(int chatRoomId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
	}

	public async Task SendPrivateMessage(int userId, string message)
	{
		if (ConnectionManager.Users.TryGetValue(userId, out var connectionId))
		{
			await Clients.Client(connectionId)
				.SendAsync("ReceivePrivateMessage", message);
		}
	}
}
