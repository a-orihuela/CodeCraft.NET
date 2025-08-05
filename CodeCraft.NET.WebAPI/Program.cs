using CodeCraft.NET.Application.Middleware;
using CodeCraft.NET.Cross.Services;
using CodeCraft.NET.Infrastructure;
using CodeCraft.NET.Infrastructure.Persistence;
using CodeCraft.NET.WebAPI.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using PARA.Platform.Application;

await DockerManager.EnsureDatabaseIsRunningAsync();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var configuration = builder.Configuration;
var (appConnection, jwtSettings, adminEmail, adminUsername, adminPassword, adminRole) = EnvironmentConfig.Load(configuration);
var environment = builder.Environment;

// 1. Register layers
builder.Services.AddInfrastructureServices(builder.Configuration, appConnection);
builder.Services.AddApplicationServices();

// 2. JWT Authentication Config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "CodeCraft.WebAPI API V1", Version = "v1" });

	// JWT Bearer setup
	var jwtSecurityScheme = new OpenApiSecurityScheme
	{
		Scheme = "bearer",
		BearerFormat = "JWT",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Description = "Enter a valid JWT token",

		Reference = new OpenApiReference
		{
			Id = JwtBearerDefaults.AuthenticationScheme,
			Type = ReferenceType.SecurityScheme
		}
	};

	c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{ jwtSecurityScheme, Array.Empty<string>() }
	});
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

// ✅ Auto-migrate in development
if (app.Environment.IsDevelopment())
{
	using var scope = app.Services.CreateScope();
	try
	{
		var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await context.Database.MigrateAsync();
		Console.WriteLine("✅ Database migrations applied successfully.");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"⚠️ Migration error: {ex.Message}");
		Console.WriteLine("⚠️ Continuing without database migrations...");
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "CodeCraft.WebAPI API V1");
		c.RoutePrefix = "swagger";
	});
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

if (!app.Environment.IsDevelopment())
{
	app.MapFallbackToFile("/index.html");
}

app.Run();