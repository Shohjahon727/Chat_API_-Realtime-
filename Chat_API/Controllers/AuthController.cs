using ChatAPI.Application.Commands;
using ChatAPI.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat_API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
	private readonly IMediator _mediator;

	public AuthController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login(LoginRequestDto request)
	{
		var command = new LoginCommand
		{
			Email = request.Email,
			Password = request.Password
		};

		var result = await _mediator.Send(command);
		return Ok(result);
	}

	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<IActionResult> Register(RegisterRequestDto request)
	{
		var command = new RegisterCommand
		{
			UserName = request.UserName,
			Email = request.Email,
			Password = request.Password,
			Avatar = request.Avatar
		};

		var result = await _mediator.Send(command);
		return Ok(result);
	}

	[HttpPost("refresh-token")]
	public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
	{
		var result = await _mediator.Send(command);
		return Ok(result);
	}
}
