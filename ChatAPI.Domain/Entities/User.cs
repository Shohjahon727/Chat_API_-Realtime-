using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Entities
{
	public class User : BaseEntity
	{
		public string UserName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string? Avatar { get; set; }
		public bool IsOnline { get; set; }
		public DateTime? LastActive { get; set; }
		public string? PasswordHash { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiry { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsActive { get; set; } = true;

		public ICollection<UserChat> UserChats { get; set; } = new List<UserChat>();
		public ICollection<Message> SentMessages { get; set; } = new List<Message>();
	}
}
