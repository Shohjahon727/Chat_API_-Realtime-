using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.DTOs
{
	public class PagedUserListDto
	{
		public List<UserDto> Users { get; set; }
		public int TotalCount { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }

		public PagedUserListDto()
		{
			Users = new List<UserDto>();
		}
	}
}
