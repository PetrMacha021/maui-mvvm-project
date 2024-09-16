using endproject.Data.Models;
using endproject.Services;
using SQLite;

namespace endproject.Data;

public class Database
{
    SQLiteAsyncConnection _database;

    public Database()
    {
    }

    async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _database.CreateTableAsync<Item>();
        await _database.CreateTableAsync<User>();

        string adminSalt = AuthService.GenerateSalt();
        string adminPassword = AuthService.HashPassword("admin", adminSalt);
        // TODO: Make this save only on database creation
        // await SaveUserAsync(new User { Username = "admin", Password = adminPassword, Salt = adminSalt });
        // await SaveItemAsync(new Item { Message = "test", OwnerId = 1 });
    }

    public List<Item> GetItems(int id)
    {
        Init().Wait();

        var task = _database.Table<Item>().Where(i => i.OwnerId == id).ToListAsync();

        task.Wait();
        return task.Result;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        await Init();

        return await _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<int> SaveItemAsync(Item item)
    {
        await Init();
        if (item.Id != 0)
            return await _database.UpdateAsync(item);

        return await _database.InsertAsync(item);
    }

    public async Task<List<User>> GetAllUsers()
    {
        await Init();

        return await _database.Table<User>().ToListAsync();
    }

    public async Task<User> GetUser(int id)
    {
        await Init();

        return await _database.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveUserAsync(User user)
    {
        await Init();
        if (user.Id != 0)
            return await _database.UpdateAsync(user);

        return await _database.InsertAsync(user);
    }
}
