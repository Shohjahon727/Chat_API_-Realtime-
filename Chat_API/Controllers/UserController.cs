using ChatAPI.Application.Commands;
using ChatAPI.Application.DTOs;
using ChatAPI.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat_API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("me")]
	public async Task<IActionResult> GetMyProfile()
	{
		var userId = int.Parse(User.FindFirst("id")?.Value);
		var query = new GetUserByIdQuery { UserId = userId };
		var result = await _mediator.Send(query);
		return Ok(result);
	}

	[HttpPut("me")]
	public async Task<IActionResult> UpdateMyProfile(UpdateUserCommand command)
	{
		var userId = int.Parse(User.FindFirst("id")?.Value);
		command.UserId = userId;
		var result = await _mediator.Send(command);
		return Ok(result);
	}

	[HttpGet("{userId}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> GetUserById(int userId)
	{
		var query = new GetUserByIdQuery { UserId = userId };
		var result = await _mediator.Send(query);
		return Ok(result);
	}
}
