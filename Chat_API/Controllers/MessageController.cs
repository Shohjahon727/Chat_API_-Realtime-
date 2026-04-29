using ChatAPI.Application.Commands;
using ChatAPI.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat_API.Controllers;

[ApiController]
[Route("api/messages")]
public class MessageController : ControllerBase
{
	private readonly IMediator _mediator;

	public MessageController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	public async Task<IActionResult> SendMessage(SendMessageCommand command)
	{
		var result = await _mediator.Send(command);
		return Ok(result);
	}

	[HttpGet]
	public async Task<IActionResult> GetMessages([FromQuery] GetMessagesQuery query)
	{
		var result = await _mediator.Send(query);
		return Ok(result);
	}
}