using ChatAPI.Application.Commands;
using ChatAPI.Application.DTOs;
using ChatAPI.Application.Interfaces;
using ChatAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatAPI.Application.Handler
{
	public class RegisterCommandHandler : IRequestHandler<RegisterCommand, TokenResponseDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;

		public RegisterCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
		{
			_unitOfWork = unitOfWork;
			_configuration = configuration;
		}

		public async Task<TokenResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
		{
			var existingUser = await _unitOfWork.Users.Query()
				.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
			if (existingUser != null)
				throw new InvalidOperationException("Email already registered");

			var existingUsername = await _unitOfWork.Users.Query()
				.FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);
			if (existingUsername != null)
				throw new InvalidOperationException("Username already taken");

			var user = new User
			{
				UserName = request.UserName.Trim(),
				Email = request.Email.Trim().ToLower(),
				PasswordHash = ComputeHash(request.Password),
				Avatar = request.Avatar,
				IsAdmin = false,
				IsActive = true,
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWork.Users.AddAsync(user, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var token = GenerateJwtToken(user);
			var refreshToken = GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(24);
			_unitOfWork.Users.Update(user);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return new TokenResponseDto
			{
				AccessToken = token.AccessToken,
				RefreshToken = refreshToken,
				AccessTokenExpiry = token.Expiry,
				RefreshTokenExpiry = user.RefreshTokenExpiry.Value,
				User = new UserDto
				{
					Id = user.Id,
					UserName = user.UserName,
					Email = user.Email,
					Avatar = user.Avatar,
					IsAdmin = user.IsAdmin,
					IsActive = user.IsActive,
					IsOnline = user.IsOnline,
					LastActive = user.LastActive
				}
			};
		}

		private string ComputeHash(string input)
		{
			using var sha256 = SHA256.Create();
			var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
			return Convert.ToBase64String(bytes);
		}

		private (string AccessToken, DateTime Expiry) GenerateJwtToken(User user)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
			var issuer = jwtSettings["Issuer"];
			var audience = jwtSettings["Audience"];
			var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim("id", user.Id.ToString()),
				new Claim("username", user.UserName),
				new Claim("email", user.Email),
				new Claim("isAdmin", user.IsAdmin.ToString())
			};

			var credentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
				signingCredentials: credentials);

			return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
		}

		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
	}
}
