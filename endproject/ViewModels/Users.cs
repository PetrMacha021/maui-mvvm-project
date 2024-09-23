using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using endproject.Data;
using endproject.Data.Models;
using endproject.Services;

namespace endproject.ViewModels;

public class Users : BindableObject {
    private readonly Database _database;
    private string _password;
    private string _username;
    private string _errorMessage;
    private ObservableCollection<User> _users;

    public Users(Database database) {
        _database = database;

        _users = new ObservableCollection<User>(Task.Run(async () => await _database.GetAllUsers()).Result);

        AddCommand = new Command(OnAdd);
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

    public ObservableCollection<User> UsersList {
        get => _users;
        set {
            _users = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddCommand { get; }

    public async void OnAdd() {
        if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password)) {
            ErrorMessage = "You have to fill in username and password";
            return;
        }

        if (UsersList.Any(u => u.Username == _username)) {
            ErrorMessage = "User with this username already exists";
            return;
        }

        var salt           = AuthService.GenerateSalt();
        var hashedPassword = AuthService.HashPassword(_password, salt);

        var user = new User {
            Username = _username,
            Password = hashedPassword,
            Salt     = salt
        };

        await _database.SaveUserAsync(user);
        UsersList.Add(user);

        Username     = "";
        Password     = "";
        ErrorMessage = "";
    }
}
