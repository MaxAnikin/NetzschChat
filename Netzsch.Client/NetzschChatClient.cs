using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Netzsch.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace Netzsch.Client;

public class NetzschChatClient
{
    private const string MessagesResource = "messages";
    private const string UsersResource = "users";

    private static readonly Lazy<NetzschChatClient> Lazy =
        new Lazy<NetzschChatClient>(() => new NetzschChatClient());

    public static NetzschChatClient Instance => Lazy.Value;

    private NetzschChatClient()
    {
    }

    private User? _user;
    private string _url;
    private string? _token;

    public User? CurrentUser => _user;
    public bool IsLogged => _user != null;

    public void Initialize(string url, CancellationToken cancellationToken)
    {
        _url = url;
    }

    private RestClient GetClient()
    {
        if (String.IsNullOrWhiteSpace(_token))
            return new RestClient(_url);

        var authenticator = new JwtAuthenticator(_token);
        var options = new RestClientOptions(_url)
        {
            Authenticator = authenticator,
            ThrowOnAnyError = false
        };

        return new RestClient(options);
    }

    public async Task<List<User>?> GetMyUsers(CancellationToken cancellationToken)
    {
        if (!IsLogged)
            return new List<User>();

        using var client = GetClient();
        var request = new RestRequest(UsersResource).AddQueryParameter("ExceptEmail", CurrentUser?.Email);
        return await client.GetAsync<List<User>>(request, cancellationToken);
    }

    public async Task SendMessage(string toEmail, string message)
    {
        if (!IsLogged)
            return;

        try
        {
            using var client = GetClient();
            var request = new RestRequest(MessagesResource).AddJsonBody(new Message()
            {
                Id = Guid.NewGuid(), ToEmail = toEmail, FromEmail = CurrentUser.Email, CreatedDate = DateTime.UtcNow,
                Text = message
            });
            var response = await client.ExecutePostAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error, message was not created.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Message>?> GetLastMessages(string toEmail)
    {
        if (!IsLogged)
            return new List<Message>();

        using var client = GetClient();
        var request = new RestRequest(MessagesResource).AddParameter("from", CurrentUser.Email)
            .AddParameter("to", toEmail);
        return await client.GetAsync<List<Message>>(request, CancellationToken.None);
    }

    public async Task CreateUser(User user)
    {
        try
        {
            using var client = GetClient();
            var request = new RestRequest(UsersResource).AddJsonBody(user);
            var response = await client.ExecutePostAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error, user was not created");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> Login(string email, string password)
    {
        using var client = GetClient();
        var request = new RestRequest("token").AddJsonBody(new TokenRequest() { Email = email, Password = password });
        var response = await client.ExecutePostAsync<string>(request);
        if (!response.IsSuccessful)
        {
            return false;
        }

        _token = response.Data;

        using var client2 = GetClient();
        var request2 = new RestRequest(UsersResource).AddQueryParameter("email", email);
        var response2 = await client2.ExecuteGetAsync<List<User>>(request2);
        _user = response2.Data?.SingleOrDefault();
        return true;
    }
}