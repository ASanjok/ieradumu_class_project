using maui_app_ieradumi_2.Services;
using maui_app_ieradumi_2.ViewModels;
using maui_app_ieradumi_2.Views;
using Microsoft.Extensions.Logging;
using static maui_app_ieradumi_2.ViewModels.AddHabitToScheduleViewModel;

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

            // Регистрация сервисов, view и ViewModel
            //-----------------------------------------------------------------
            builder.Services.AddSingleton<IDataService, DataService>();

            builder.Services.AddTransient<AuthViewModel>();
            builder.Services.AddTransient<HabitListViewModel>();
            builder.Services.AddTransient<EditHabitViewModel>();
            builder.Services.AddTransient<AddHabitViewModel>();
            builder.Services.AddTransient<UserInfoViewModel>();
            builder.Services.AddTransient<ScheduleViewModel>(); 
            builder.Services.AddTransient<AddHabitToScheduleViewModel>();

            builder.Services.AddTransient<AuthPage>();
            builder.Services.AddTransient<HabitListPage>();
            builder.Services.AddTransient<EditHabitPage>();
            builder.Services.AddTransient<AddHabitPage>();
            builder.Services.AddTransient<UserInfoPage>();
            builder.Services.AddTransient<SchedulePage>();
            builder.Services.AddTransient<AddHabitToSchedulePage>();
            builder.Services.AddTransient<DeleteHabitFromSchedulePage>();

            builder.Services.AddTransient<Views.Elements.HabitInfoElement>();
            //-----------------------------------------------------------------

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
