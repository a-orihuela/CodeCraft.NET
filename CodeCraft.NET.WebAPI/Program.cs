using CodeCraft.NET.Application.Middleware;
using CodeCraft.NET.Infrastructure;
using CodeCraft.NET.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PARA.Platform.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var configuration = builder.Configuration;
var appConnection = configuration.GetConnectionString("Application") ?? 
    throw new InvalidOperationException("Missing 'ConnectionStrings:Application'. Please set it via environment or appsettings.");
var environment = builder.Environment;

// 1. Register layers
builder.Services.AddInfrastructureServices(builder.Configuration, appConnection);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI API V1", Version = "v1" });
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", builder =>
	{
		if (environment.IsDevelopment())
		{
			builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
		}
		else
		{
			builder
				.WithOrigins("https://site.com")
				.AllowAnyMethod()
				.AllowAnyHeader();
		}
	});
});

var app = builder.Build();

// Auto-migrate in development
if (app.Environment.IsDevelopment())
{
	using var scope = app.Services.CreateScope();
	try
	{
		var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await context.Database.MigrateAsync();
		Console.WriteLine("Database migrations applied successfully.");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Migration error: {ex.Message}");
		Console.WriteLine("Continuing without database migrations...");
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI API V1");
		c.RoutePrefix = "swagger";
	});
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();

if (!app.Environment.IsDevelopment())
{
	app.MapFallbackToFile("/index.html");
}

app.Run();