using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.DTOs
{
	public class UserDto
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string? Avatar { get; set; }
		public bool IsOnline { get; set; }
		public DateTime? LastActive { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsActive { get; set; }
	}
}
