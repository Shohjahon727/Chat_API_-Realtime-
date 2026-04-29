using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Entities
{
	public class Message : BaseEntity
	{
		public string Text { get; set; }

		public int SenderId { get; set; }
		public User Sender { get; set; }

		public int ChatRoomId { get; set; }
		public ChatRoom ChatRoom { get; set; }
	}
}
