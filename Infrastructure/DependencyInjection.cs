using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Phần Kết Nối Database

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseOracle(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        #endregion
        
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWebService, WebService>();
        services.AddScoped<IInitDataService, InitDataService>();
        services.AddScoped<IGitService, GitService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICommonService, CommonService>();
        services.AddScoped<ITableService, TableService>();
        services.AddScoped<IQRCodeService, QRCodeService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ITableBookingService, TableBookingService>();
        
        return services;
    }
}