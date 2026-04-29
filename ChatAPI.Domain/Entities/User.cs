using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Entities
{
	public class User : BaseEntity
	{
		public string UserName { get; set; }

		public ICollection<UserChat> UserChats { get; set; }
		public ICollection<Message> SentMessages { get; set; }
	}
}
