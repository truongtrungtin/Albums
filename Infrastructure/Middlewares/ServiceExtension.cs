using Core.Entities.Identity;
using Core.Helpers;
using Core.Interfaces;
using Infrastructure.Data.Library;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.ViewModel;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middlewares
{
    public static class ServiceExtension
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
        }
        public static void UseLoggingRequestMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }

    }

    public static class ServicesConfiguration
    {

        public static void AddUnitOfWork(this IServiceCollection services)
        {

            //services.AddSingleton<EntityDataContext>();
            //services.AddSingleton<UnitOfWork>();
            //services.AddScoped<TokenSettings>();
            services.AddScoped<UnitOfWork>();
            services.AddScoped<UserTokens>();
            services.AddScoped<IUserService, UserService>();


        }
        public static void AddSyncData(this IServiceCollection services)
        {

        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUploadFilesLibrary, UploadFilesLibrary>();

        }

        public static void AddCustomServices(this IServiceCollection services)
        {

        }
    }
}
