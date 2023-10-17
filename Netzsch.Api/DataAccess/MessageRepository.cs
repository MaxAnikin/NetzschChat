using LiteDB;
using Netzsch.Models;

namespace Netzsch.Api.DataAccess;

public class MessageRepository : IMessageRepository
{
    private readonly string? _connectionString;

    public MessageRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(Constants.ChatDbConnectionStringName);
    }
    
    public bool Insert(Message message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        using var db = new LiteDatabase(_connectionString);

        return db.GetCollection<Message>().Upsert(message);
    }

    public bool Update(Message message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        using var db = new LiteDatabase(_connectionString);

        return db.GetCollection<Message>().Upsert(message);

    }

    public int Delete(Message message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        using var db = new LiteDatabase(_connectionString);
        return db.GetCollection<Message>().DeleteMany(x => x.CreatedDate == message.CreatedDate && x.FromEmail == message.FromEmail && x.ToEmail == message.ToEmail);
    }

    public IEnumerable<Message> Get(string from, string to)
    {
        using var db = new LiteDatabase(_connectionString);
        var messages = db.GetCollection<Message>().Query().ToList();
        return messages.Where(x =>
            (x.FromEmail == from && x.ToEmail == to) || (x.FromEmail == to && x.ToEmail == from));
    }
}

public interface IMessageRepository : IRepository<Message>
{
    IEnumerable<Message> Get(string from, string to);
}