using endproject.Data;
using endproject.Services;

namespace endproject.Pages;

public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
        var database = new Database();
        var authService = new AuthService(database);
        BindingContext = new ViewModels.Login(authService);
    }
}
