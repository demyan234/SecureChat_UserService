using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SecureChatUserMicroService.Application.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            
            //TODO: Регистрируем MediatR и другие сервисы
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}