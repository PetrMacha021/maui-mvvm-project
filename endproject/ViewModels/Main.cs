﻿using System.Collections.ObjectModel;
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
    private Item? _selectedItem;

    public ObservableCollection<Item> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged();
        }
    }

    public Item? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            if (_selectedItem == null) return;
            Title = _selectedItem.Title;
            Message = _selectedItem.Message;

            OnPropertyChanged();
        }
    }

    public ICommand AddCommand { get; }
    public ICommand RemoveItem { get; }
    public ICommand EditItem { get; }

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

    private string _title;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    private bool _isMessageVisible;

    public bool IsMessageVisible
    {
        get => _isMessageVisible;
        set
        {
            _isMessageVisible = value;
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
        EditItem = new Command(OnEditItem);
    }

    private async void OnAdd()
    {
        var newItem = new Item()
        {
            Title = _title,
            Message = _message,
            OwnerId = _id
        };
        await _database.SaveItemAsync(newItem);

        Items.Add(newItem);

        Title = "";
        Message = "";
    }

    private async void OnRemoveItem(object parameter)
    {
        if (parameter is not int id) return;

        await _database.RemoveItem(id);

        var itemToRemove = Items.FirstOrDefault(item => item.Id == id);
        if (itemToRemove != null) Items.Remove(itemToRemove);
    }

    private void OnEditItem(object parameter)
    {
        if (parameter is not Item item) return;
        SelectedItem = item;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
