using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Entities
{
	public class Message : BaseEntity
	{
		public string Text { get; set; } = string.Empty;
		public string? MediaType { get; set; }
		public string? MediaUrl { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }

		public int SenderId { get; set; }
		public User Sender { get; set; } = null!;

		public int ChatRoomId { get; set; }
		public ChatRoom ChatRoom { get; set; } = null!;
	}
}
