using CleanArch.App;
using CleanArch.App.MiddleWare;
using CleanArch.App.Resources;
using CleanArch.Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace CleanArch.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // SERVICES CONFIGURATION

            // Base Services
            builder.Services.AddControllers();

            // LOCALIZATION CONFIGURATION

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            // MVC with Localization
            builder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    // Using SharedResource instead of JsonStringLocalizerFactory for Data Annotations
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            // Supported Cultures Configuration
            var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("ar-EG") };
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
            });

            // SWAGGER/OPENAPI CONFIGURATION

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CleanArch API",
                    Version = "v1",
                    Description = "Clean Architecture API"
                });

                // JWT Authentication Scheme for Swagger
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Enter your JWT Access Token",
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

            // APPLICATION & INFRASTRUCTURE LAYERS

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddHttpContextAccessor();

            // APPLICATION BUILD & MIDDLEWARE PIPELINE

            var app = builder.Build();

            // DEVELOPMENT ENVIRONMENT SETUP

            if (app.Environment.IsDevelopment())
            {
                // Swagger UI Configuration
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArch v1");
                    c.RoutePrefix = string.Empty;  // Set Swagger as root
                    c.DisplayRequestDuration();
                });
            }

            // FILE UPLOADS DIRECTORY SETUP

            var env = app.Services.GetRequiredService<IWebHostEnvironment>();
            var uploadsRoot = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"),
                builder.Configuration.GetSection("UploadOptions:BaseFolder").Value ?? "Uploads");
            Directory.CreateDirectory(uploadsRoot);

            // MIDDLEWARE PIPELINE CONFIGURATION

            // Forwarded Headers for Proxy Scenarios
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Static Files Serving
            app.UseStaticFiles();

            // Custom Exception Handling
            app.UseGlobalExceptionHandling();

            // Security
            app.UseHttpsRedirection();

            // Localization Middleware
            var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(locOptions);

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Endpoint Mapping
            app.MapControllers();

            // Application Start
            app.Run();
        }
    }
}