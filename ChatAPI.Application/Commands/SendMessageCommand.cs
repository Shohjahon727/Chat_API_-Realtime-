using MediatR;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Commands
{
	public class SendMessageCommand : IRequest<int>
	{
		public string Text { get; set; }
		public int SenderId { get; set; }
		public int ChatRoomId { get; set; }
		
	}
}
