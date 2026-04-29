using Chat_API.Hubs;
using Chat_API.Middleware;
using ChatAPI.Application.Interfaces;
using ChatAPI.Application.MediatR;
using ChatAPI.Application.Validators;
using ChatAPI.Infrastructure.Data;
using ChatAPI.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Services
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Chat API",
		Version = "v1"
	});

	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Bearer token kiriting. Misol: Bearer abc123",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	
});

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Disambiguate AddMediatR overload by passing the assembly explicitly so the Assembly[] params overload is chosen.
	builder.Services.AddMediatR(cfg =>
	cfg.WithEvaluator(type => type.Assembly == typeof(ApplicationAssemblyMarker).Assembly),
	typeof(ApplicationAssemblyMarker).Assembly);

builder.Services.AddSignalR();

builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		var jwtSettings = builder.Configuration.GetSection("JwtSettings");
		var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(key),
			ClockSkew = TimeSpan.Zero
		};

		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				var accessToken = context.Request.Query["access_token"];
				var path = context.HttpContext.Request.Path;

				if (!string.IsNullOrEmpty(accessToken) &&
					path.StartsWithSegments("/chatHub"))
				{
					context.Token = accessToken;
				}

				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();