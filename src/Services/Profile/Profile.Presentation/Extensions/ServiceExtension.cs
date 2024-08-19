using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Profile.Domain.Interfaces.Services;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations.Services;
using Shared.Options;

namespace Profile.Presentation.Extensions;

public static class ServiceExtension
{
    public static void ConfigurePresentation(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthorization();
        services.AddControllers();
        services.ConfigureJwtOptions(config);
        services.ConfigureMinioOptions(config);
        services.ConfigureAuthentication(services.BuildServiceProvider().GetService<IOptions<JwtOptions>>()!.Value);
        services.ConfigureMinio(services.BuildServiceProvider().GetService<IOptions<MinioOptions>>()!.Value);
        services.ConfigureCors();
        services.ConfigureSwagger();
        services.ConfigureRedisCache(config);
    }

    private static void ConfigureMinio(this IServiceCollection services, MinioOptions minioOptions)
    {
        services.AddSingleton<IMinioService>(provider => new MinioService(minioOptions.Endpoint, minioOptions.AccessKey, 
            minioOptions.SecretKey, minioOptions.BucketName));
    }
    
    private static void ConfigureRedisCache(this IServiceCollection services, IConfiguration config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config["Redis:Server"];
        });
    }
    
    private static void ConfigureJwtOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(config.GetSection("ApiSettings:JwtOptions"));
    }
    
    private static void ConfigureMinioOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MinioOptions>(config.GetSection("Minio"));
    }
    
    private static void ConfigureAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
    {
        var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtOpt =>
        {
            jwtOpt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference= new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=JwtBearerDefaults.AuthenticationScheme
                        }
                    }, new string[]{}
                }
            });
        });
    }
    
    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("MyCorsPolicy", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }
    
    public static void ApplyMigrations(this IApplicationBuilder app, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();

        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }
    }
}