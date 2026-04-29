using ChatAPI.Application.Commands;
using ChatAPI.Application.DTOs;
using ChatAPI.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Handler
{
	public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.Query()
				.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

			if (user == null)
				throw new KeyNotFoundException($"User with ID {request.UserId} not found");

			if (!string.IsNullOrEmpty(request.UserName))
				user.UserName = request.UserName.Trim();

			if (!string.IsNullOrEmpty(request.Email))
			{
				var existingEmail = await _unitOfWork.Users.Query()
					.FirstOrDefaultAsync(u => u.Email == request.Email && u.Id != request.UserId, cancellationToken);
				if (existingEmail != null)
					throw new InvalidOperationException("Email already in use");

				user.Email = request.Email.Trim().ToLower();
			}

			if (!string.IsNullOrEmpty(request.Avatar))
				user.Avatar = request.Avatar;

			if (request.IsAdmin.HasValue)
				user.IsAdmin = request.IsAdmin.Value;

			if (request.IsActive.HasValue)
				user.IsActive = request.IsActive.Value;

			user.UpdatedAt = DateTime.UtcNow;

			_unitOfWork.Users.Update(user);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

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
