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

    public Main(Database database)
    {
        _database = database;

        var id = Task.Run(async () => await SecureStorage.Default.GetAsync("auth_id")).Result;
        if (id == null)
        {
            OnLogout();
            return;
        }

        _id = int.Parse(id);

        _items = new ObservableCollection<Item>(_database.GetItems(_id));

        LogoutCommand = new Command(OnLogout);
        AddCommand = new Command(OnAdd);
    }

    private void OnLogout()
    {
        SecureStorage.Default.Remove("auth_id");
        App.Current.MainPage = new AppShell();
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
