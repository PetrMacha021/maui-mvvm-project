using endproject.Data;
using endproject.Pages.Main;
using endproject.Services;
using endproject.ViewModels;
using Microsoft.Extensions.Logging;
using Profile = endproject.ViewModels.Profile;
using Users = endproject.ViewModels.Users;

namespace endproject;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<Database>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddTransient<Login>();
        builder.Services.AddTransient<Main>();
        builder.Services.AddTransient<Users>();
        builder.Services.AddTransient<Profile>();
        builder.Services.AddTransient<Pages.Login>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Pages.Main.Users>();
        builder.Services.AddTransient<Pages.Main.Profile>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}