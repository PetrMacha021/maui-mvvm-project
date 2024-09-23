using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using endproject.Data;
using endproject.Data.Models;
using endproject.Services;

namespace endproject.ViewModels;

public class Profile : BindableObject {
    private readonly Database _database;
    private readonly int _id;
    private readonly User _user;
    private string _username;
    private string _password;
    private string _errorMessage;
    private string _successMessage;

    public Profile(Database database) {
        _database = database;

        ChangeCommand = new Command(OnChange);
        LogoutCommand = new Command(Logout);

        var id = Task.Run(async () => await SecureStorage.Default.GetAsync("auth_id")).Result;
        if (id == null) {
            Application.Current.MainPage = new AppShell();
            return;
        }

        _id = int.Parse(id);

        var user = Task.Run(async () => await _database.GetUser(_id)).Result;
        if (user == null) {
            Logout();
            return;
        }

        Username = user.Username;
        _user    = user;
    }

    public string Username {
        get => _username;
        set {
            _username    = value;
            ErrorMessage = "";
            SuccessMessage = "";
            OnPropertyChanged();
        }
    }

    public string Password {
        get => _password;
        set {
            _password      = value;
            ErrorMessage   = "";
            SuccessMessage = "";
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

    public string SuccessMessage {
        get => _successMessage;
        set {
            _successMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand ChangeCommand { get; }
    public ICommand LogoutCommand { get; }

    private async void OnChange() {
        var changed = false;

        if (string.IsNullOrEmpty(_username) && string.IsNullOrEmpty(_password)) {
            ErrorMessage = "Fill in at least one entry";
            return;
        }

        if (_user.Username != _username) {
            var existingUser = Task.Run(() => _database.GetUserByUsernameAsync(_username)).Result;
            if (existingUser != null) {
                ErrorMessage = "User with this username already exists";
                return;
            }

            _user.Username = _username;
            changed        = true;
        }

        if (!string.IsNullOrEmpty(_password)) {
            var salt           = AuthService.GenerateSalt();
            var hashedPassword = AuthService.HashPassword(_password, salt);

            _user.Password = hashedPassword;
            _user.Salt     = salt;
            changed        = true;
        }

        if (changed) {
            await _database.SaveUserAsync(_user);
            ErrorMessage   = "";
            SuccessMessage = "Profile has been updated";
        }
        else ErrorMessage = "Nothing changed";
    }

    public static void Logout() {
        SecureStorage.Default.Remove("auth_id");
        Application.Current.MainPage = new AppShell();
    }
}
