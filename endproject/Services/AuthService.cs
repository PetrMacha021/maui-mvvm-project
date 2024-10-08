﻿using System.Security.Cryptography;
using System.Text;
using endproject.Data;
using endproject.Data.Models;

namespace endproject.Services;

public class AuthService {
    private readonly Database _database;

    public AuthService(Database database) {
        _database = database;
    }

    public static string GenerateSalt(int size = 32) {
        var rng    = new Random();
        var buffer = new byte[size];
        rng.NextBytes(buffer);
        return Convert.ToBase64String(buffer);
    }

    public static string HashPassword(string password, string salt) {
        using (var sha512 = SHA512.Create()) {
            var saltedPassword      = $"{password}{salt}";
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashedBytes         = sha512.ComputeHash(saltedPasswordBytes);
            var sb                  = new StringBuilder();
            foreach (var b in hashedBytes) sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }

    public async Task<(bool isValid, User?)> ValidateCredentialsAsync(string username, string password) {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return (false, null);

        var user = await _database.GetUserByUsernameAsync(username);
        if (user == null) return (false, null);

        var isValid = user.Password == HashPassword(password, user.Salt);
        return (isValid, user);
    }
}
