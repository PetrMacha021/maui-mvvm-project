using System.ComponentModel;
using System.Windows.Input;
using endproject.Services;

namespace endproject.ViewModels;

public class Login : BindableObject
{
    private string _username;
    private string _password;
    private string _errorMessage;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; }

    private AuthService _authService;

    public Login(AuthService authService)
    {
        _authService = authService;
        LoginCommand = new Command(OnLogin);
    }

    private async void OnLogin()
    {
        var (isAuthenticated, user) = await _authService.ValidateCredentialsAsync(Username, Password);

        if (isAuthenticated && user != null)
        {
            await SecureStorage.Default.SetAsync("auth_id", user.Id.ToString());
            await Shell.Current.GoToAsync("///Main");
        }
        else
        {
            ErrorMessage = "Invalid username or password";
        }
    }
}
