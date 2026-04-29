using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.DTOs
{
	public class PagedResponseDto<T>
	{
		public List<T> Data { get; set; }
		public int TotalCount { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

		public PagedResponseDto()
		{
			Data = new List<T>();
		}
	}
}
