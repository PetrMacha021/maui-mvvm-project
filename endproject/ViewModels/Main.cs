using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using endproject.Data;
using endproject.Data.Models;

namespace endproject.ViewModels;

public class Main : BindableObject {
    private bool _isMessageVisible;
    private ObservableCollection<Item> _items;

    private string _message;
    private Item? _selectedItem;
    private string _title;
    private string _errorMessage;

    private bool _inEdit;

    public bool InEdit {
        get => _inEdit;
        set {
            _inEdit = value;
            OnPropertyChanged();
        }
    }

    public Main(Database database) {
        _database = database;

        var id = Task.Run(async () => await SecureStorage.Default.GetAsync("auth_id")).Result;
        if (id == null) {
            Profile.Logout();
            return;
        }

        _id = int.Parse(id);

        _items = new ObservableCollection<Item>(_database.GetItems(_id));

        AddCommand = new Command(OnAdd);
        RemoveItem = new Command(OnRemoveItem);
        EditItem   = new Command(OnEditItem);
        Cancel     = new Command(OnCancel);
    }

    private Database _database { get; }
    private int _id { get; }

    public ObservableCollection<Item> Items {
        get => _items;
        set {
            _items = value;
            OnPropertyChanged();
        }
    }

    public Item? SelectedItem {
        get => _selectedItem;
        set {
            _selectedItem = value;
            if (_selectedItem == null) return;
            Title   = _selectedItem.Title;
            Message = _selectedItem.Message;

            OnPropertyChanged();
        }
    }

    public ICommand AddCommand { get; }
    public ICommand RemoveItem { get; }
    public ICommand EditItem { get; }
    public ICommand Cancel { get; }

    public string Message {
        get => _message;
        set {
            _message     = value;
            ErrorMessage = "";
            OnPropertyChanged();
        }
    }

    public string Title {
        get => _title;
        set {
            _title       = value;
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

    public bool IsMessageVisible {
        get => _isMessageVisible;
        set {
            _isMessageVisible = value;
            OnPropertyChanged();
        }
    }

    private async void OnAdd() {
        if (string.IsNullOrEmpty(_title) || string.IsNullOrEmpty(_message)) {
            ErrorMessage = "You have to fill in both entries in to add an item.";
            return;
        }

        if (InEdit) {
            await OnEdit();
            InEdit       = false;
            Title        = "";
            Message      = "";
            SelectedItem = null;
            return;
        }

        var newItem = new Item {
            Title   = _title,
            Message = _message,
            OwnerId = _id
        };
        await _database.SaveItemAsync(newItem);

        Items.Add(newItem);

        Title        = "";
        Message      = "";
        ErrorMessage = "";
    }

    private async Task OnEdit() {
        if (SelectedItem == null) {
            ErrorMessage = "No item selected";
            return;
        }

        SelectedItem.Title   = _title;
        SelectedItem.Message = _message;

        await _database.SaveItemAsync(SelectedItem);

        Items = new ObservableCollection<Item>(_database.GetItems(_id));
    }

    private async void OnRemoveItem(object parameter) {
        if (parameter is not int id) return;

        await _database.RemoveItem(id);

        var itemToRemove = Items.FirstOrDefault(item => item.Id == id);
        if (itemToRemove != null) Items.Remove(itemToRemove);
    }

    private void OnEditItem(object parameter) {
        if (parameter is not Item item) return;
        InEdit       = true;
        SelectedItem = item;
    }

    private void OnCancel() {
        InEdit       = false;
        Title        = "";
        Message      = "";
        SelectedItem = null;
    }
}
