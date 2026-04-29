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
	public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;

		public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
		{
			_unitOfWork = unitOfWork;
			_configuration = configuration;
		}

		public async Task<TokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
		{
			var principal = GetPrincipalFromExpiredToken(request.AccessToken);
			var userId = int.Parse(principal.FindFirst("id")?.Value);

			var user = await _unitOfWork.Users.Query()
				.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

			if (user == null || user.RefreshToken != request.RefreshToken)
				throw new UnauthorizedAccessException("Invalid refresh token");

			if (user.RefreshTokenExpiry <= DateTime.UtcNow)
				throw new UnauthorizedAccessException("Refresh token has expired");

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

		private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ClockSkew = TimeSpan.Zero,
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key)
			};

			var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
			return principal;
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
