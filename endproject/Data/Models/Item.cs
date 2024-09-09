using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace endproject.Data.Models;

[SQLite.Table("Items")]
public class Item
{
    [PrimaryKey, AutoIncrement]
    [SQLite.Column("id")]
    public int Id { get; set; }

    [SQLite.Column("message")]
    public string Message { get; set; } = default!;

    [SQLite.Column("owner_id"), ForeignKey("OwnerId")]
    public int OwnerId { get; set; }
}
