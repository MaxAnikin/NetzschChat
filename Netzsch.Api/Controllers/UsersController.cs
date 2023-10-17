using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netzsch.Api.DataAccess;
using Netzsch.Models;

namespace Netzsch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController: ControllerBase
{
    private readonly ILogger<MessagesController> _logger;
    private readonly IUserRepository _userRepository;

    public UsersController(ILogger<MessagesController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet()]
    public async Task<IActionResult> Get([FromQuery]UserFilter? filter)
    {
        await Task.CompletedTask;
        return new JsonResult(_userRepository.Get(filter));
    }

    [HttpPost()]
    [AllowAnonymous]
    public async Task<IActionResult> Post(User user)
    {
        try
        {
            await Task.CompletedTask;
            _userRepository.Insert(user);
            return new OkResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPut()]
    public async Task<IActionResult> Put(User user)
    {
        await Task.CompletedTask;
        _userRepository.Update(user);
        return new OkResult();
    }
   
    [HttpDelete()]
    public async Task<IActionResult> Delete(string email)
    {
        await Task.CompletedTask;
        _userRepository.Delete(email);
        return new OkResult();
    }
}