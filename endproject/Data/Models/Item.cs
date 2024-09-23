using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SQLite;

namespace endproject.Data.Models;

[SQLite.Table("Items")]
public class Item : INotifyPropertyChanged {
    private bool _isMessageVisible;

    [PrimaryKey]
    [AutoIncrement]
    [SQLite.Column("id")]
    public int Id { get; set; }

    [SQLite.Column("title")] public string Title { get; set; } = default!;

    [SQLite.Column("message")] public string Message { get; set; } = default!;

    [SQLite.Column("owner_id")]
    [ForeignKey("OwnerId")]
    public int OwnerId { get; set; }

    [Ignore]
    public bool IsMessageVisible {
        get => _isMessageVisible;
        set {
            _isMessageVisible = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
