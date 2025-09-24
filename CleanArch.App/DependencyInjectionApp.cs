using CleanArch.App.Features.Users.Commands.UpdateProfile;
using CleanArch.App.Interface;
using CleanArch.App.Interface.Auth;
using CleanArch.App.Interface.Storage;
using CleanArch.App.Services;
using CleanArch.App.Services.Auth;
using CleanArch.App.Services.Storage;
using CleanArch.Infra.Options;
using CleanArch.Infra.Services;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArch.App
{
    public static class DependencyInjectionApp
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            // MediatR Registration (v12+)
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // ✅ Fix: use configuration instead of config
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IResponseModel, ResponseModel>();
            services.AddScoped<IEmailService, EmailService>();
            services.Configure<UploadOptions>(configuration.GetSection("UploadOptions"));
            services.AddScoped<IFileStorage, LocalFileStorage>();
            services.AddScoped<ILocationService, LocationService>();


            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(UserMappingConfig).Assembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();


            // Validators, Mapping ... إلخ
            return services;
        }
    }
}
