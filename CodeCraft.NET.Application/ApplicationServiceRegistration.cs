using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CodeCraft.NET.Application.Behaviours;
using System.Reflection;

namespace PARA.Platform.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
			services.AddAutoMapper(config =>
			{
				config.AddMaps(Assembly.GetExecutingAssembly());
			});
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }
    }
}
