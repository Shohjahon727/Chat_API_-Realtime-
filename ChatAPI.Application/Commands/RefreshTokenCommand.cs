using ChatAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class RefreshTokenCommand : IRequest<TokenResponseDto>
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
