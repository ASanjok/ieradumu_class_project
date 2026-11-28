using maui_app_ieradumi_2.Services;
using maui_app_ieradumi_2.ViewModels;
using maui_app_ieradumi_2.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;

namespace maui_app_ieradumi_2
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


            string baseAddress = DeviceInfo.Platform == DevicePlatform.Android
                ? "https://10.0.2.2:7103/"   // Для Android-эмулятора localhost ПК доступен по IP 10.0.2.2
                : "https://localhost:7103/"; // Для Windows / iOS Simulator

            builder.Services.AddHttpClient<IDataService, DataService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromSeconds(15);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                // Разрешаем локальный SSL-сертификат разработки (localhost)
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
            });

            // Регистрация сервисов, view и ViewModel
            //-----------------------------------------------------------------
            //builder.Services.AddSingleton<IDataService, DataService>();

            builder.Services.AddTransient<AuthViewModel>();
            builder.Services.AddTransient<HabitListViewModel>();
            builder.Services.AddTransient<EditHabitViewModel>();
            builder.Services.AddTransient<AddHabitViewModel>();
            builder.Services.AddTransient<UserInfoViewModel>();
            builder.Services.AddTransient<ScheduleViewModel>();
            builder.Services.AddTransient<AddHabitToScheduleViewModel>();
            builder.Services.AddTransient<CompletionViewModel>();

            builder.Services.AddTransient<AuthPage>();
            builder.Services.AddTransient<HabitListPage>();
            builder.Services.AddTransient<EditHabitPage>();
            builder.Services.AddTransient<AddHabitPage>();
            builder.Services.AddTransient<UserInfoPage>();
            builder.Services.AddTransient<SchedulePage>();
            builder.Services.AddTransient<AddHabitToSchedulePage>();
            builder.Services.AddTransient<DeleteHabitFromSchedulePage>();
            builder.Services.AddTransient<CompletionPage>();

            builder.Services.AddTransient<Views.Elements.HabitInfoElement>();
            //-----------------------------------------------------------------

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
