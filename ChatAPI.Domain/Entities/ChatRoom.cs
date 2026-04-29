using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Entities
{
	public class ChatRoom : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? Icon { get; set; }
		public bool IsPrivate { get; set; }
		public int CreatedById { get; set; }
		public User CreatedBy { get; set; } = null!;

		public ICollection<UserChat> UserChats { get; set; } = new List<UserChat>();
		public ICollection<Message> Messages { get; set; } = new List<Message>();
	}
}
