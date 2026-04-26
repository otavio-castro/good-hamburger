using GoodHamburgerFront.Services;

namespace GoodHamburgerFront.Startup
{
    public static class DIStartup
    {
        public static void ConfigureServices(IHostApplicationBuilder builder)
        {
            #region Razor Pages

            builder.Services.AddRazorPages();

            #endregion Razor Pages

            #region Http

            builder.Services.AddHttpClient();

            #endregion Http

            #region Services

            builder.Services.AddScoped<IGoodHamburgerApiService, GoodHamburgerApiService>();

            #endregion Services
        }
    }
}
