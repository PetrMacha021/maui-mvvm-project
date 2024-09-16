using System.Collections.ObjectModel;
using System.ComponentModel;
using endproject.Data;
using endproject.Data.Models;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace endproject.ViewModels;

public class Main : BindableObject
{
    private Database _database { get; }
    private int _id { get; }
    private ObservableCollection<Item> _items;

    public ObservableCollection<Item> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddCommand { get; }
    public ICommand RemoveItem { get; }

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

    public Main(Database database)
    {
        _database = database;

        var id = Task.Run(async () => await SecureStorage.Default.GetAsync("auth_id")).Result;
        if (id == null)
        {
            Profile.Logout();
            return;
        }

        _id = int.Parse(id);

        _items = new ObservableCollection<Item>(_database.GetItems(_id));

        AddCommand = new Command(OnAdd);
        RemoveItem = new Command(OnRemoveItem);
    }

    private async void OnAdd()
    {
        var newItem = new Item()
        {
            Message = _message,
            OwnerId = _id
        };
        await _database.SaveItemAsync(newItem);

        Items.Add(newItem);

        Message = "";
    }

    private async void OnRemoveItem(object parameter)
    {
        if (parameter is not int id) return;

        await _database.RemoveItem(id);

        var itemToRemove = Items.FirstOrDefault(item => item.Id == id);
        if (itemToRemove != null) Items.Remove(itemToRemove);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
