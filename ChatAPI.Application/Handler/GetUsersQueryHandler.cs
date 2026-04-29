using ChatAPI.Application.DTOs;
using ChatAPI.Application.Interfaces;
using ChatAPI.Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Handler
{
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedUserListDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetUsersQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<PagedUserListDto> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			var query = _unitOfWork.Users.Query()
				.AsNoTracking();

			if (!string.IsNullOrEmpty(request.SearchTerm))
			{
				query = query.Where(u => u.UserName.Contains(request.SearchTerm) || u.Email.Contains(request.SearchTerm));
			}

			if (request.IsAdmin.HasValue)
			{
				query = query.Where(u => u.IsAdmin == request.IsAdmin.Value);
			}

			if (request.IsActive.HasValue)
			{
				query = query.Where(u => u.IsActive == request.IsActive.Value);
			}

			var totalCount = await query.CountAsync(cancellationToken);

			var users = await query
				.OrderBy(u => u.UserName)
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(u => new UserDto
				{
					Id = u.Id,
					UserName = u.UserName,
					Email = u.Email,
					Avatar = u.Avatar,
					IsAdmin = u.IsAdmin,
					IsActive = u.IsActive,
					IsOnline = u.IsOnline,
					LastActive = u.LastActive
				})
				.ToListAsync(cancellationToken);

			return new PagedUserListDto
			{
				Users = users,
				TotalCount = totalCount,
				PageNumber = request.PageNumber,
				PageSize = request.PageSize,
				TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
			};
		}
	}
}
