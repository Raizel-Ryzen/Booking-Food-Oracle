using System.Text;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace WebUI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        
        services.AddRazorPages()
            .AddRazorRuntimeCompilation();

        services.AddControllersWithViews();

        services.AddHttpContextAccessor();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(6);
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options=> {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });
            // .AddGoogle(options =>
            // {
            //     options.ClientId = configuration["Google:AppId"];
            //     options.ClientSecret = configuration["Google:AppSecret"];
            //     options.ClaimActions.MapJsonKey("Picture", "picture", "url");
            //     options.SaveTokens = true;
            //     options.CallbackPath = configuration["Google:CallbackPath"];
            // })
            // .AddFacebook(options =>
            // {
            //     options.AppId = configuration["Facebook:AppId"];
            //     options.AppSecret = configuration["Facebook:AppSecret"];
            //     options.SaveTokens = true;
            //     options.CallbackPath = configuration["Facebook:CallbackPath"];
            // });

        //ISS
        services.Configure<IISServerOptions>(options => { options.MaxRequestBodySize = int.MaxValue; });
        //Kestrel
        services.Configure<KestrelServerOptions>(options => { options.Limits.MaxRequestBodySize = int.MaxValue; });
        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = int.MaxValue;
            options.MultipartHeadersLengthLimit = int.MaxValue;
        });

        return services;
    }
}