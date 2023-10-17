using LiteDB;

namespace Netzsch.Models;

public class User
{
    static User()
    {
        var mapper = BsonMapper.Global;

        mapper.Entity<User>()
            .Id(x => x.Email);
    }
    
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Online { get; set; }
}