using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scrutor;
using System.Text;
using TransportApp.Application.Interfaces;
using TransportApp.Application.Services;
using TransportApp.Domain.Model;
using TransportApp.Persistence;

namespace TransportApp;

public static class ServiceExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration conf)
    {
        // Add DbContext with options from configuration
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(conf.GetConnectionString("DefaultConnection")));

        // Add Identity services
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedEmail = false;

            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });
        return services;
    }
    public static void ConfigureJWT(this IServiceCollection services, IConfiguration conf)
    {
        #region JWT
        var TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = conf["JwtSettings:Audience"],
            ValidIssuer = conf["JwtSettings:Site"],
            RequireExpirationTime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["JwtSettings:Secret"])),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(TokenValidationParameters);

        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = TokenValidationParameters;
        });
        #endregion
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        // Add CORS policy
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });
        return services;

    }

    public static IServiceCollection AddScrutor(this IServiceCollection services)
    {
        services.Scan(scan =>
               scan.FromAssemblyOf<ITokenGenerator>()
                  .AddClasses(
                        classSelector =>
                            classSelector.InNamespaces("TransportApp.Application")
                  )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
           );
        return services;
    }
}
