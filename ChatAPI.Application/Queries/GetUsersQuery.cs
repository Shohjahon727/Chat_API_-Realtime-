using ChatAPI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Queries
{
	public class GetUsersQuery : IRequest<PagedUserListDto>
	{
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 20;
		public string? SearchTerm { get; set; }
		public bool? IsAdmin { get; set; }
		public bool? IsActive { get; set; }
	}
}
