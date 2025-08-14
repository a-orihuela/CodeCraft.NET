using CodeCraft.NET.Application.Contracts.Persistence;
using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeCraft.NET.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string appConnection)
		{
			AddDbContext(services, configuration, appConnection);
			AddRepositories(services);

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

		private static void AddDbContext(IServiceCollection services, IConfiguration configuration, string appConnection)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					appConnection,
					x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
				)
			);
		}
	}
}
