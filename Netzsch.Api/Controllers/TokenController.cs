using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Netzsch.Api.DataAccess;
using Netzsch.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Netzsch.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController: ControllerBase
{
    public IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public TokenController(IConfiguration config, IUserRepository userRepository)
    {
        _configuration = config;
        _userRepository = userRepository;
    }

    [HttpPost]
    public IActionResult Post([FromBody]TokenRequest tokenRequest)
    {
        if (tokenRequest == null) throw new ArgumentNullException(nameof(tokenRequest));

        var tokenHandler = new JwtSecurityTokenHandler();
        var user = _userRepository.Get(tokenRequest.Email, tokenRequest.Password);
        if (user == null)
            return Unauthorized();
        
        {
            //create claims details based on the user information
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, tokenRequest.Email),
                new Claim(JwtRegisteredClaimNames.Email, tokenRequest.Email),
                new Claim("userid", tokenRequest.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            
            return Ok(jwt);
        }
    }
}