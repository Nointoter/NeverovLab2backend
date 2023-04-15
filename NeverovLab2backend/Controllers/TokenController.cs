using NeverovLab2backend.Models;
using NeverovLab2backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeverovLab2backend.Data;
using Microsoft.Net.Http.Headers;

namespace NeverovLab2backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly DBHelper _db;
    private readonly pgDbContext _dbContext;
    private readonly ITokenService _tokenService;

    public TokenController(pgDbContext pgDbContext, ITokenService tokenService)
    {
        _db = new DBHelper(pgDbContext);
        _dbContext = pgDbContext;
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh(TokenApiModel tokenApiModel)
    {

        User user = _db.GetUserByToken(tokenApiModel.AccessToken);

        if (tokenApiModel is null)
            return BadRequest("Invalid client request");

        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name; 

        if (user == null || user.RefreshToken != refreshToken || Convert.ToDateTime(user.TokenExpires) <= DateTime.Now)
            return BadRequest("Invalid client request");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.Token = newAccessToken;
        user.RefreshToken = newRefreshToken;
        _dbContext.SaveChanges();

        return Ok(new TokenApiModel()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost, Authorize]
    [Route("revoke")]
    public IActionResult Revoke()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
        var row = _db.GetUserByToken(accessToken);
       
        if (row == null) return BadRequest();
        row.RefreshToken = null;
        _dbContext.SaveChanges();
        return NoContent();
    }
}
