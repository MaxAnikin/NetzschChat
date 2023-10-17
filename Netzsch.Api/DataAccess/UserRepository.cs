using System.Globalization;
using CryptoHelper;
using LiteDB;
using Netzsch.Models;

namespace Netzsch.Api.DataAccess;

public class UserRepository : IUserRepository
{
    private readonly string? _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(Constants.ChatDbConnectionStringName);
    }
    
    public IEnumerable<User> Get(UserFilter? filter)
    {
        using var db = new LiteDatabase(_connectionString);

        var users = db.GetCollection<User>().Query().ToList();
        
        if (filter == null || (filter.Email == null && filter.ExceptEmail == null))
            return users;

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            return users.Where(x => x.Email == filter.Email).ToList();
        }

        return users.Where(x => x.Email != filter.ExceptEmail).ToList();
    }

    public User? Get(string email, string password)
    {
        using var db = new LiteDatabase(_connectionString);
        var users = db.GetCollection<User>().Query().ToList();
        var user = users.SingleOrDefault(x => x.Email == email);
        
        if (!Crypto.VerifyHashedPassword(user?.Password, password))
            return null;

        return user;
    }

    public bool Insert(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (String.IsNullOrWhiteSpace(user.Email)) throw new ArgumentException("User email is invalid.");

        using var db = new LiteDatabase(_connectionString);
        if (db.GetCollection<User>().Query().Where(x => x.Email == user.Email).SingleOrDefault() != null)
        {
            throw new Exception($"User \"{user.Email}\" does not exist.");
        }

        user.Password = Crypto.HashPassword(user.Password);
        
        return db.GetCollection<User>().Upsert(user);
    }

    public bool Update(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (String.IsNullOrWhiteSpace(user.Email)) throw new ArgumentException("User email is invalid.");

        using var db = new LiteDatabase(_connectionString);
        return db.GetCollection<User>().Update(user);
    }

    public int Delete(User user)
    {
        using var db = new LiteDatabase(_connectionString);
        var result = db.GetCollection<User>().DeleteMany(x => x.Email == user.Email);
        db.Commit();
        return result;
    }

    public int Delete(string email)
    {
        using var db = new LiteDatabase(_connectionString);
        var result = db.GetCollection<User>().DeleteMany(x => x.Email == email);
        db.Commit();
        return result;
    }
}

public interface IUserRepository : IRepository<User>
{
    IEnumerable<User> Get(UserFilter? filter);
    int Delete(string email);
    User? Get(string email, string password);
}