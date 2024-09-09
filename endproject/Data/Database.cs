using endproject.Data.Models;
using SQLite;

namespace endproject.Data;

public class Database
{
    SQLiteAsyncConnection _database;

    public Database()
    {
    }

    void Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        _database.CreateTableAsync<Item>().Wait();
        _database.CreateTableAsync<User>().Wait();
    }

    public List<Item> GetItemsAsync(int id)
    {
        Init();

        var task = _database.Table<Item>().Where(i => i.Id == id).ToListAsync();

        task.Wait();
        return task.Result;
    }

    public Task<User> GetUserByUsernameAsync(string username)
    {
        Init();

        return _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<int> SaveItemAsync(Item item)
    {
        Init();
        if (item.Id != 0)
            return await _database.UpdateAsync(item);

        return await _database.InsertAsync(item);
    }
}
