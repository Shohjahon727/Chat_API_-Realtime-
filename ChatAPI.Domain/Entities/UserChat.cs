using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Entities
{
	public class UserChat : BaseEntity
	{
		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public int ChatRoomId { get; set; }
		public ChatRoom ChatRoom { get; set; } = null!;
	}
}
