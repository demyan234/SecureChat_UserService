using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureChatUserMicroService.Application.Application.Extensions;

namespace SecureChatUserMicroService.Infrastructure.Extensions;

public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCollectionInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddDbContext<UserServiceDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlDatabase")));

            services.AddScoped<UserServiceDbContext>(provider => provider.GetService<UserServiceDbContext>()
                                                              ?? throw new InvalidOperationException());

            //TODO: Регистрация  Mediator(-a)
            
            
            //TODO: Репозитории

            services.AddApplication();

            return services;
        }
    }