using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netzsch.Api.DataAccess;
using Netzsch.Models;

namespace Netzsch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly IMessageRepository _messageRepository;

    public MessagesController(ILogger<MessagesController> logger, IMessageRepository messageRepository)
    {
        _logger = logger;
        _messageRepository = messageRepository;
    }
    
    [HttpGet()]
    public async Task<IEnumerable<Message>> Get(string from, string to)
    {
        await Task.CompletedTask;
        return _messageRepository.Get(from, to);
    }
    
    [HttpPost()]
    public async Task<IActionResult> Post(Message message)
    {
        await Task.CompletedTask;
        _messageRepository.Insert(message);
        return new OkResult();
    }
}