using Chat_API.Realtime;
using ChatAPI.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat_API.Hubs;

public class ChatHub : Hub
{
	private readonly IMediator _mediator;
	private readonly ILogger<ChatHub> _logger;

	public ChatHub(IMediator mediator, ILogger<ChatHub> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	public override async Task OnConnectedAsync()
	{
		_logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);

		try
		{
			var userId = int.Parse(Context.User.FindFirst("id")?.Value ?? "0");

			if (userId > 0)
			{
				ConnectionManager.Users[userId] = Context.ConnectionId;
				_logger.LogInformation("User {UserId} connected with connection {ConnectionId}", userId, Context.ConnectionId);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in OnConnectedAsync");
		}

		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception exception)
	{
		_logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);

		if (exception != null)
		{
			_logger.LogError(exception, "Connection closed with exception");
		}

		try
		{
			var userId = int.Parse(Context.User.FindFirst("id")?.Value ?? "0");

			if (userId > 0)
			{
				ConnectionManager.Users.TryRemove(userId, out _);
				_logger.LogInformation("User {UserId} disconnected", userId);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in OnDisconnectedAsync");
		}

		await base.OnDisconnectedAsync(exception);
	}

	public async Task SendMessage(SendMessageCommand command, CancellationToken cancellationToken = default)
	{
		try
		{
			// 1. DB ga yozish
			var messageId = await _mediator.Send(command, cancellationToken);

			// 2. realtime yuborish
			await Clients.Group(command.ChatRoomId.ToString())
				.SendAsync("ReceiveMessage", new
				{
					command.Text,
					command.SenderId,
					command.ChatRoomId,
					MessageId = messageId,
					CreatedAt = DateTime.UtcNow
				}, cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in SendMessage");
			throw;
		}
	}

	public async Task JoinRoom(int chatRoomId, CancellationToken cancellationToken = default)
	{
		try
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString(), cancellationToken);
			_logger.LogInformation("User joined room {ChatRoomId}", chatRoomId);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in JoinRoom");
			throw;
		}
	}

	public async Task LeaveRoom(int chatRoomId, CancellationToken cancellationToken = default)
	{
		try
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString(), cancellationToken);
			_logger.LogInformation("User left room {ChatRoomId}", chatRoomId);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in LeaveRoom");
			throw;
		}
	}

	public async Task SendPrivateMessage(int userId, string message, CancellationToken cancellationToken = default)
	{
		try
		{
			if (ConnectionManager.Users.TryGetValue(userId, out var connectionId))
			{
				await Clients.Client(connectionId)
					.SendAsync("ReceivePrivateMessage", message, cancellationToken);
				_logger.LogInformation("Private message sent to user {UserId}", userId);
			}
			else
			{
				_logger.LogWarning("User {UserId} is not online", userId);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in SendPrivateMessage");
			throw;
		}
	}
}
