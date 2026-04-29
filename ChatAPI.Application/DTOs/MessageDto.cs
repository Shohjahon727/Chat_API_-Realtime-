using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.DTOs
{
	public class MessageDto
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public int SenderId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
