using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Services;
using E_Commerce.Infastructure.Data;
using E_Commerce.Infastructure.Repositries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infastructure
{
    public static class InfastructureRegistation
    {
        public static IServiceCollection InfrastructureConfiguration (this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IgenricRepository<>), typeof(genricRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Cs")));
            services.AddSingleton<IImageManagementService, ImageManagementService>();
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            return services;
        }
    }
}
