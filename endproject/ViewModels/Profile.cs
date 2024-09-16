using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using endproject.Data;
using endproject.Data.Models;
using endproject.Services;

namespace endproject.ViewModels;

public class Profile: BindableObject
{
    private Database _database;
    private int _id;
    private User _user;
    private string _username;
    private string _password;

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

    public ICommand ChangeCommand { get; }
    public ICommand LogoutCommand { get; }

    private async void OnChange()
    {
        if (_user.Username != _username)
        {
            _user.Username = _username;
        }

        var salt = AuthService.GenerateSalt();
        var hashedPassword = AuthService.HashPassword(_password, salt);

        _user.Password = hashedPassword;
        _user.Salt = salt;

        await _database.SaveUserAsync(_user);
    }

    public static void Logout()
    {
        SecureStorage.Default.Remove("auth_id");
        App.Current.MainPage = new AppShell();
    }

    public Profile(Database database)
    {
        _database = database;

        ChangeCommand = new Command(OnChange);
        LogoutCommand = new Command(Logout);

        var id = Task.Run(async () => await SecureStorage.Default.GetAsync("auth_id")).Result;
        if (id == null)
        {
            App.Current.MainPage = new AppShell();
            return;
        }

        _id = int.Parse(id);

        var user = Task.Run(async () => await _database.GetUser(_id)).Result;

        Username = user.Username;
        _user = user;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
