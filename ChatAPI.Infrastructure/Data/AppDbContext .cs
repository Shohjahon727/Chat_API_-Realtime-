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

			// User entity configuration
			modelBuilder.Entity<User>(entity =>
			{
				entity.Property(u => u.UserName)
					.IsRequired()
					.HasMaxLength(50);

				entity.Property(u => u.Email)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(u => u.PasswordHash)
					.HasMaxLength(256);

				entity.Property(u => u.Avatar)
					.HasMaxLength(500);

				entity.HasIndex(u => u.Email)
					.IsUnique();

				entity.HasIndex(u => u.UserName)
					.IsUnique();
			});

			// ChatRoom entity configuration
			modelBuilder.Entity<ChatRoom>(entity =>
			{
				entity.Property(c => c.Name)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(c => c.Description)
					.HasMaxLength(500);

				entity.Property(c => c.Icon)
					.HasMaxLength(500);
			});

			// Message entity configuration
			modelBuilder.Entity<Message>(entity =>
			{
				entity.Property(m => m.Text)
					.IsRequired()
					.HasMaxLength(5000);

				entity.Property(m => m.MediaType)
					.HasMaxLength(50);

				entity.Property(m => m.MediaUrl)
					.HasMaxLength(500);

				entity.HasIndex(m => m.ChatRoomId);
				entity.HasIndex(m => m.SenderId);
				entity.HasIndex(m => m.CreatedAt);
			});

			// UserChat entity configuration
			modelBuilder.Entity<UserChat>(entity =>
			{
				entity.HasKey(uc => new { uc.UserId, uc.ChatRoomId });

				entity.HasOne(uc => uc.User)
					.WithMany(u => u.UserChats)
					.HasForeignKey(uc => uc.UserId);

				entity.HasOne(uc => uc.ChatRoom)
					.WithMany(c => c.UserChats)
					.HasForeignKey(uc => uc.ChatRoomId);
			});

			
		}
	}
}
