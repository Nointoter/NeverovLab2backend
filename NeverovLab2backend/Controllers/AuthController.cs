using NeverovLab2backend.Models;
using NeverovLab2backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models.Auth;
using System.Security.Cryptography;


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
    [HttpPost("register")]
    public async Task<ActionResult<UserModel>> Register(UserDto request)
    {
        try
        {

            UserModel user = new UserModel();
            if (!_db.CheackUserName(request.Username))
                return BadRequest("Такой пользователь уже существует");
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Id = 0;
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _db.SaveUser(user);
            //Занести данные в таблицу User(UserModel), UserDto c помощью dbHelper

            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] UserDto loginModel)
    {
        User user = _db.GetUserByUserName(loginModel.Username);

        if (loginModel is null)
        {
            return BadRequest("Отсутствуют данные.");
        }
        if (user.Username != loginModel.Username || !VerifyPasswordHash(loginModel.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Введён неверный логин или пароль.");
        }
            
           
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.Username),
            new Claim(ClaimTypes.Role, "Player")
        };
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.Token = accessToken;
        user.RefreshToken = refreshToken;
        user.TokenCreated = Convert.ToString(DateTime.Now);
        user.TokenExpires = Convert.ToString(DateTime.Now.AddMinutes(1));
        

        _dbContext.SaveChanges();

        return Ok(new AuthenticatedResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken
        });
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
