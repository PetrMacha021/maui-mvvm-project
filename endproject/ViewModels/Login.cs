using System.Windows.Input;
using endproject.Pages;
using endproject.Services;

namespace endproject.ViewModels;

public class Login : BindableObject {
    private readonly AuthService _authService;
    private string _errorMessage;
    private string _password;
    private string _username;

    public Login(AuthService authService) {
        _authService = authService;
        LoginCommand = new Command(OnLogin);
    }

    public string Username {
        get => _username;
        set {
            _username = value;
            ErrorMessage = "";
            OnPropertyChanged();
        }
    }

    public string Password {
        get => _password;
        set {
            _password = value;
            ErrorMessage = "";
            OnPropertyChanged();
        }
    }

    public string ErrorMessage {
        get => _errorMessage;
        set {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; }

    private async void OnLogin() {
        var (isAuthenticated, user) = await _authService.ValidateCredentialsAsync(Username, Password);

        Password = "";

        if (isAuthenticated && user != null) {
            await SecureStorage.Default.SetAsync("auth_id", user.Id.ToString());
            Application.Current.MainPage = new MainShell();
        }
        else {
            ErrorMessage = "Invalid username or password";
        }
    }
}
