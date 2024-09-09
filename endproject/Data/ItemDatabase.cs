using endproject.Data.Models;
using SQLite;

namespace endproject.Data;

public class ItemDatabase
{
    SQLiteAsyncConnection _database;

    public ItemDatabase()
    {
    }

    void Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        _database.CreateTableAsync<Item>().Wait();
    }

    public List<Item> GetItemsAsync()
    {
        Init();

        var task = _database.Table<Item>().ToListAsync();

        task.Wait();
        return task.Result;
    }

    public async Task<int> SaveItemAsync(Item item)
    {
        Init();
        if (item.Id != 0)
            return await _database.UpdateAsync(item);

        return await _database.InsertAsync(item);
    }
}
