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
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.Query()
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

			if (user == null)
				throw new KeyNotFoundException($"User with ID {request.UserId} not found");

			return new UserDto
			{
				Id = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Avatar = user.Avatar,
				IsAdmin = user.IsAdmin,
				IsActive = user.IsActive,
				IsOnline = user.IsOnline,
				LastActive = user.LastActive
			};
		}
	}
}
