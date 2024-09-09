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
        builder.Services.AddSingleton<ViewModels.Login>();
        builder.Services.AddSingleton<ViewModels.Main>();
        builder.Services.AddSingleton<Pages.MainPage>();
        builder.Services.AddSingleton<Pages.Login>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
