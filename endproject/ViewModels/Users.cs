using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using endproject.Data;
using endproject.Data.Models;
using endproject.Services;

namespace endproject.ViewModels;

public class Users: BindableObject
{
    private Database _database;
    private ObservableCollection<User> _users;
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

    public ObservableCollection<User> UsersList
    {
        get => _users;
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddCommand { get; }

    public async void OnAdd()
    {
        var salt = AuthService.GenerateSalt();
        var hashedPassword = AuthService.HashPassword(_password, salt);

        var user = new User()
        {
            Username = _username,
            Password = hashedPassword,
            Salt = salt
        };

        await _database.SaveUserAsync(user);
        UsersList.Add(user);

        Username = "";
        Password = "";
    }

    public Users(Database database)
    {
        _database = database;

        _users = new ObservableCollection<User>(Task.Run(async () => await _database.GetAllUsers()).Result);

        AddCommand = new Command(OnAdd);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
