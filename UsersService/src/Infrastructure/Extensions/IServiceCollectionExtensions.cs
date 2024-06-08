using Application.Interfaces.Services;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            AddInfrastructureLayerServices(services);
        }

        private static void AddInfrastructureLayerServices(this IServiceCollection services)
        {
            services.AddTransient<IMediator, Mediator>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
