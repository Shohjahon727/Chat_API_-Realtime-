using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.DTOs
{
	public class TokenResponseDto
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime AccessTokenExpiry { get; set; }
		public DateTime RefreshTokenExpiry { get; set; }
		public UserDto User { get; set; }
	}
}
