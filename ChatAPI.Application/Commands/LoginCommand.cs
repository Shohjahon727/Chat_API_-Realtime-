using ChatAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class LoginCommand : IRequest<TokenResponseDto>
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
