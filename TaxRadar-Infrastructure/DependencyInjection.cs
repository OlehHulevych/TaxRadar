using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tax_Radar_Domain.Entities;
using TaxRadar_Application.Interfaces;
using TaxRadar_Infrastructure.Persistance;
using TaxRadar_Infrastructure.Repository;
using TaxRadar_Infrastructure.Services;

namespace TaxRadar_Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((options) =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IRepository<User>, UserRepository>();
        services.AddScoped<IRepository<Client>, ClientRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        return services;
    }
}