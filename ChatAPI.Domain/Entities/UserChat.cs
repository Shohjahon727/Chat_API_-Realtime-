using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Entities
{
	public class UserChat
	{
		public int UserId { get; set; }
		public User User { get; set; }

		public int ChatRoomId { get; set; }
		public ChatRoom ChatRoom { get; set; }
	}
}
