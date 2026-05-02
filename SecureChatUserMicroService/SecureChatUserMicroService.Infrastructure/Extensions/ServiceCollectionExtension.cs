using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureChatUserMicroService.Application.Application.Extensions;
using SecureChatUserMicroService.Application.Common.Interfaces;
using SecureChatUserMicroService.Application.Common.Interfaces.IGrpcClients;
using SecureChatUserMicroService.Application.Common.Interfaces.IRepository;
using SecureChatUserMicroService.Application.GrpcServices.Clients;
using SecureChatUserMicroService.Infrastructure.Repositories;

namespace SecureChatUserMicroService.Infrastructure.Extensions
{
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

            //Регистрация  Mediator(-a)
            
            
            //Репозитории
            services.AddScoped<IUserServiceDbContext, UserServiceDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            //Grpc Clients Interfaces
            services.AddScoped<IChatGrpcClient, ChatGrpcClientService>();

            services.AddApplication();

            return services;
        }
    }
}