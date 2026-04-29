using ChatAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Application.Interfaces
{
	public interface IUnitOfWork
	{
		IGenericRepository<User> Users { get; }
		IGenericRepository<Message> Messages { get; }
		IGenericRepository<ChatRoom> ChatRooms { get; }
		IGenericRepository<UserChat> UserChats { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
