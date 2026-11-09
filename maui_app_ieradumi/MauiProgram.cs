using maui_app_ieradumi.ViewModels;
using Microsoft.Extensions.Logging;
using maui_app_ieradumi.services;

namespace maui_app_ieradumi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<IDataService, DataService>();

            builder.Services.AddTransient<mainViewModel>();
            builder.Services.AddTransient<loginViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoginPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
