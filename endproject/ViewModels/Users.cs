using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using endproject.Data;
using endproject.Data.Models;

namespace endproject.ViewModels;

public class Users: BindableObject
{
    private Database _database;
    private ObservableCollection<User> _users;

    public ObservableCollection<User> UsersList
    {
        get => _users;
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }

    public ICommand UserAddCommand { get; }

    public Users(Database database)
    {
        _database = database;

        _users = new ObservableCollection<User>(Task.Run(async () => await _database.GetAllUsers()).Result);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
