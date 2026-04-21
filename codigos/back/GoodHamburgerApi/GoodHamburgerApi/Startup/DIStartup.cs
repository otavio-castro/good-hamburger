using GoodHamburgerApi.Context;
using GoodHamburgerApi.Repositories;
using GoodHamburgerApi.Repositories.Interfaces;
using GoodHamburgerApi.Services;
using GoodHamburgerApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerApi.Startup
{
    public static class DIStartup
    {
        public static void ConfigureServices(IHostApplicationBuilder builder)
        {
            #region DbContext

            builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseInMemoryDatabase("GoodHamburgerDb"));

            #endregion DbContext

            #region AutoMapper

            builder.Services.AddAutoMapper(cfg => 
            {
                cfg.AddMaps(typeof(DIStartup).Assembly);
            });

            #endregion AutoMapper

            #region Repositories

            builder.Services.AddScoped<ICardapioRepository, CardapioRepository>();
            builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

            #endregion Repositories

            #region Services

            builder.Services.AddScoped<IPedidoService, PedidoService>();
            builder.Services.AddScoped<ICardapioService, CardapioService>();

            #endregion Services
        }
    }
}