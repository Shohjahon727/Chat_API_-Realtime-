using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Entities
{
	public class ChatRoom : BaseEntity
	{
		public string Name { get; set; }

		public ICollection<UserChat> UserChats { get; set; }
		public ICollection<Message> Messages { get; set; }
	}
}
