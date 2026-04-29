using ChatAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<ChatRoom> ChatRooms { get; set; }
		public DbSet<UserChat> UserChats { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserChat>()
				.HasKey(uc => new { uc.UserId, uc.ChatRoomId });

			modelBuilder.Entity<UserChat>()
				.HasOne(uc => uc.User)
				.WithMany(u => u.UserChats)
				.HasForeignKey(uc => uc.UserId);

			modelBuilder.Entity<UserChat>()
				.HasOne(uc => uc.ChatRoom)
				.WithMany(c => c.UserChats)
				.HasForeignKey(uc => uc.ChatRoomId);
		}
	}
}
