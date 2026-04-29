using ChatAPI.Application.Commands;
using ChatAPI.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Handler
{
	public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.Query()
				.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

			if (user == null)
				throw new KeyNotFoundException($"User with ID {request.UserId} not found");

			_unitOfWork.Users.Delete(user);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return true;
		}
	}
}
