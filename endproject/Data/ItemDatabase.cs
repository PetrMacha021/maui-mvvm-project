using endproject.Data.Models;
using SQLite;

namespace endproject.Data;

public class ItemDatabase
{
    SQLiteAsyncConnection _database;

    public ItemDatabase()
    {
    }

    async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await _database.CreateTableAsync<Item>();
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        await Init();
        return await _database.Table<Item>().ToListAsync();
    }

    public async Task<int> SaveItemAsync(Item item)
    {
        await Init();
        if (item.Id != 0)
            return await _database.UpdateAsync(item);

        return await _database.InsertAsync(item);
    }
}
