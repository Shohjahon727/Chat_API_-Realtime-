using ChatAPI.Application.Commands;
using ChatAPI.Application.DTOs;
using ChatAPI.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat_API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
	private readonly IMediator _mediator;

	public AdminController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("users")]
	public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
	{
		var result = await _mediator.Send(query);
		return Ok(result);
	}

	[HttpGet("users/{userId}")]
	public async Task<IActionResult> GetUserById(int userId)
	{
		var query = new GetUserByIdQuery { UserId = userId };
		var result = await _mediator.Send(query);
		return Ok(result);
	}

	[HttpPut("users/{userId}")]
	public async Task<IActionResult> UpdateUser(int userId, UpdateUserCommand command)
	{
		command.UserId = userId;
		var result = await _mediator.Send(command);
		return Ok(result);
	}

	[HttpDelete("users/{userId}")]
	public async Task<IActionResult> DeleteUser(int userId)
	{
		var command = new DeleteUserCommand { UserId = userId };
		var result = await _mediator.Send(command);
		return Ok(result);
	}
}
