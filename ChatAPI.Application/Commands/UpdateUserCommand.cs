using ChatAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class UpdateUserCommand : IRequest<UserDto>
	{
		public int UserId { get; set; }
		public string? UserName { get; set; }
		public string? Email { get; set; }
		public string? Avatar { get; set; }
		public bool? IsAdmin { get; set; }
		public bool? IsActive { get; set; }
	}
}
