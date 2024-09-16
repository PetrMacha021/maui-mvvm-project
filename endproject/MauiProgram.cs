using endproject.Data;
using endproject.Services;
using Microsoft.Extensions.Logging;

namespace endproject;

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

        builder.Services.AddSingleton<Database>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddTransient<ViewModels.Login>();
        builder.Services.AddTransient<ViewModels.Main>();
        builder.Services.AddTransient<ViewModels.Users>();
        builder.Services.AddTransient<ViewModels.Profile>();
        builder.Services.AddTransient<Pages.Login>();
        builder.Services.AddTransient<Pages.Main.MainPage>();
        builder.Services.AddTransient<Pages.Main.Users>();
        builder.Services.AddTransient<Pages.Main.Profile>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
