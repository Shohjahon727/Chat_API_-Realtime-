using ChatAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Queries
{
	public class GetUserByIdQuery : IRequest<UserDto>
	{
		public int UserId { get; set; }
	}
}
