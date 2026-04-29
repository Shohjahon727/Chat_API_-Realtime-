using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.DTOs
{
	public class ChatRoomDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? Icon { get; set; }
		public bool IsPrivate { get; set; }
		public int CreatedById { get; set; }
		public string CreatedByName { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
