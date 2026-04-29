using ChatAPI.Application.Interfaces;
using ChatAPI.Domain.Entities;
using ChatAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly AppDbContext _context;

		public IGenericRepository<User> Users { get; }
		public IGenericRepository<Message> Messages { get; }
		public IGenericRepository<ChatRoom> ChatRooms { get; }
		public IGenericRepository<UserChat> UserChats { get; }

		public UnitOfWork(AppDbContext context)
		{
			_context = context;

			Users = new GenericRepository<User>(_context);
			Messages = new GenericRepository<Message>(_context);
			ChatRooms = new GenericRepository<ChatRoom>(_context);
			UserChats = new GenericRepository<UserChat>(_context);
		}

		public async Task<int> SaveChangesAsync()
			=> await _context.SaveChangesAsync();

		public void Dispose()
		{
			_context.Dispose();
			GC.SuppressFinalize(this);
		}

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
