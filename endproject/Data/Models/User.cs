using System.ComponentModel.DataAnnotations;
using SQLite;

namespace endproject.Data.Models;

[Table("Users")]
public class User {
    [PrimaryKey]
    [AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [Unique]
    [Display(Prompt = "admin", Name = "Username")]
    public string Username { get; set; }

    [Column("password")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Column("salt")] public string Salt { get; set; }
}