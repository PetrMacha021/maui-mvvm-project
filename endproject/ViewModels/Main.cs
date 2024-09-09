using System.ComponentModel;
using endproject.Data;
using endproject.Data.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace endproject.ViewModels;

public class Main: INotifyPropertyChanged
{
    private Database _database { get; }
    private int _id { get; }
    private List<Item> _items;

    public List<Item> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged();
        }
    }

    public ICommand LogoutCommand { get; }
    public ICommand AddCommand { get; }

    private string _message;

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    public Main()
    {
        _database = new Database();

        var idTask = SecureStorage.Default.GetAsync("auth_id");
        idTask.Wait();

        _id = int.Parse(idTask.Result ?? string.Empty);

        _items = _database.GetItems(_id);

        LogoutCommand = new Command(OnLogout);
        AddCommand = new Command(OnAdd);
    }

    private async void OnLogout()
    {
        SecureStorage.Default.Remove("auth_id");
        await Shell.Current.GoToAsync("///Login");
    }

    private async void OnAdd()
    {
        var newItem = new Item()
        {
            Id = 0,
            Message = _message,
            OwnerId = _id
        };
        await _database.SaveItemAsync(newItem);

        Items = _database.GetItems(_id);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
