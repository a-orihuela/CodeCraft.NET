using CodeCraft.NET.Application.Contracts.Identity;
using CodeCraft.NET.Application.Contracts.Persistence;
using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Application.DTOs.Identity;
using CodeCraft.NET.Infrastructure.Persistence;
using CodeCraft.NET.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CodeCraft.NET.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string appConnection)
		{
			services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
			services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

			AddDbContext(services, configuration, appConnection);
			AddIdentity(services);
			AddRepositories(services);

			services.AddTransient<IAuthService, AuthService>();

			return services;
		}

		private static void AddRepositories(IServiceCollection services)
		{
			// Register generic adapter
			services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepositoryAdapter<>));
			services.AddScoped<ICodeCraftUnitOfWork, CodeCraftUnitOfWork>();

			// Automatically register all repositories in the target namespace
			var assembly = typeof(RepositoryBase<,>).Assembly;

			var repositoryTypes = assembly.GetTypes()
				.Where(t =>
					t.IsClass &&
					!t.IsAbstract &&
					t.Name.EndsWith("Repository") &&
					!t.Name.StartsWith("<") &&
					t.Namespace != null &&
					t.Namespace.EndsWith(".Persistence.Repositories"))
				.ToList();

			foreach (var implementationType in repositoryTypes)
			{
				var interfaceType = implementationType.GetInterfaces().FirstOrDefault(i =>
					i.Namespace != null &&
					i.Namespace.StartsWith("CodeCraft.NET.Application.Contracts.Persistence.Repositories"));

				if (interfaceType != null)
				{
					services.TryAddScoped(interfaceType, implementationType);
				}
			}
		}

		private static void AddIdentity(IServiceCollection services)
		{
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
		}

		private static void AddDbContext(IServiceCollection services, IConfiguration configuration, string appConnection)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseNpgsql(
					appConnection,
					x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
				)
			);
		}
	}
}
