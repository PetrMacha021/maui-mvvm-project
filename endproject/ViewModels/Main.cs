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

    public List<Item> Items { get; set; }

    public ICommand LogoutCommand { get; }

    public Main()
    {
        _database = new Database();

        var idTask = SecureStorage.Default.GetAsync("auth_id");
        idTask.Wait();

        _id = int.Parse(idTask.Result ?? string.Empty);

        Items = _database.GetItemsAsync(_id);

        LogoutCommand = new Command(OnLogout);
    }

    private async void OnLogout()
    {
        SecureStorage.Default.Remove("auth_id");
        await Shell.Current.GoToAsync("///Login");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
