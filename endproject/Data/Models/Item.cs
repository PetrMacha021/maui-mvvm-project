using SQLite;

namespace endproject.Data.Models;

[Table("Items")]
public class Item
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("message")]
    public string Message { get; set; } = default!;
}
