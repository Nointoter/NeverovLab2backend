using NeverovLab2backend.Models;
using NeverovLab2backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NeverovLab2backend.Data;
using System;
using Microsoft.EntityFrameworkCore;
using NeverovLab2backend.Models.Auth;

namespace NeverovLab2backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DBHelper _db;
    private readonly pgDbContext _dbContext;
    private readonly ITokenService _tokenService;

    public AuthController(pgDbContext pgDbContext, ITokenService tokenService)
    {
        _db = new DBHelper(pgDbContext);
        _dbContext = pgDbContext;
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] UserDto loginModel)
    {
        if (loginModel is null)
        {
            return BadRequest("Invalid client request");
        }
        /*
        var user = _dbContext.Users.FirstOrDefault(u => 
            (u.UserName == loginModel.UserName) && (u.Password == loginModel.Password));
        if (user is null)
            return Unauthorized();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.UserName),
            new Claim(ClaimTypes.Role, "Manager")
        };
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        _dbContext.SaveChanges();*/

        return Ok(new AuthenticatedResponse
        {
            Token = "",
            RefreshToken = ""
        });
    }
}
