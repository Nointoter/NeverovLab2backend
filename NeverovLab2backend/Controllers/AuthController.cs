using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;
using NeverovLab2backend.Models.Auth;
using NeverovLab2backend.Services.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace NeverovLab2backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //public static UserModel user = new UserModel();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        private readonly DBHelper _db;

        public AuthController(IConfiguration configuration, IUserService userService, pgDbContext pgDbContext)
        {
            _configuration = configuration;
            _userService = userService;
            _db = new DBHelper(pgDbContext);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register(UserDto request)
        {
            UserModel user = new UserModel();
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Id = 0;
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _db.SaveUser(user);
            //Занести данные в таблицу User(UserModel), UserDto c помощью dbHelper

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            UserModel user =  _db.GetUserByUserName(request.Username);
            
            if (user.Username != request.Username)
            {
                return BadRequest("Введён неверный логин или пароль.");
            }
            
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Введён неверный логин или пароль.");
            }

            string token = CreateToken(user);
            user.Token = token;

            var refreshToken = GenerateRefreshToken();
            user = SetRefreshToken(refreshToken, user);
            _db.SaveUser(user);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken(string refreshToken)
        {
            UserModel user = _db.GetUserByRefreshToken(refreshToken);

            if (user == null)
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(Convert.ToDateTime(user.TokenExpires) < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            user.Token = CreateToken(user);

            var newRefreshToken = GenerateRefreshToken();
            user = SetRefreshToken(newRefreshToken, user);
            _db.SaveUser(user);

            return Ok(user.Token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMinutes(30),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private UserModel SetRefreshToken(RefreshToken newRefreshToken, UserModel user)
        {

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = Convert.ToString(newRefreshToken.Created);
            user.TokenExpires = Convert.ToString(newRefreshToken.Expires);
            return (user);
        }

        private string CreateToken(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
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
}
