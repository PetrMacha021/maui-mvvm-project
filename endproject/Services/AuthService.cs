using endproject.Data;
using endproject.Data.Models;

namespace endproject.Services;

public class AuthService
{
    private readonly Database _database;

    public AuthService(Database database)
    {
        _database = database;
    }

    public async Task<(bool isValid, User?)> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _database.GetUserByUsernameAsync(username);
        if (user == null) return (false, null);

        var isValid = user.Password == password;
        return (isValid, isValid ? user : null);
    }
}
