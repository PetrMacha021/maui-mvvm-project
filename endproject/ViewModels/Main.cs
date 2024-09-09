using System.ComponentModel;
using endproject.Data;
using endproject.Data.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace endproject.ViewModels;

public class Main: INotifyPropertyChanged
{
    private ItemDatabase _database { get; }

    public List<Item> Items { get; set; }

    public Main()
    {
        _database = new ItemDatabase();
        Items = _database.GetItemsAsync();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
