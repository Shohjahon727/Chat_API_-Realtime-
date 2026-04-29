using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class DeleteUserCommand : IRequest<bool>
	{
		public int UserId { get; set; }
	}
}
