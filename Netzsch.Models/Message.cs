using LiteDB;

namespace Netzsch.Models;

public class Message
{
    static Message()
    {
        var mapper = BsonMapper.Global;

        mapper.Entity<Message>()
            .Id(x => x.Id);
    }
    
    public Guid Id { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string ToEmail { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}