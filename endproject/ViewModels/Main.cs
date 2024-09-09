using System.ComponentModel;
using endproject.Data;
using endproject.Data.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace endproject.ViewModels;

public class Main: INotifyPropertyChanged
{
    private Database _database { get; }
    private int _id { get; }

    public List<Item> Items { get; set; }

    public Main()
    {
        _database = new Database();

        var idTask = SecureStorage.Default.GetAsync("auth_id");
        idTask.Wait();

        _id = int.Parse(idTask.Result ?? string.Empty);

        Items = _database.GetItemsAsync(_id);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
