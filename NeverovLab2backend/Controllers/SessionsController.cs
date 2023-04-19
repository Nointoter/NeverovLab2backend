using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;
using NeverovLab2backend.Services;


namespace NeverovLab2backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : Controller
{
    private readonly DBHelper _db;
    private readonly ITokenService _tokenService;
    public SessionsController(pgDbContext pgDbContext, ITokenService tokenService)
    {
        _db = new DBHelper(pgDbContext);
        _tokenService = tokenService;
    }

    // POST 
    [HttpPost]
    [Route("SaveSession")]
    public IActionResult Post(SessionModel model)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            User user = _db.GetUserByToken(accessToken);
            if (user == null)
            {
                return StatusCode(501, "Token does not exist");
            }
            var isTokenWork = _tokenService.CheckTime(_db.GetUserByToken(accessToken));
            if (!isTokenWork)
            {
                return StatusCode(502, "Token does not work");
            }
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            CharacterModel characterModel = _db.GetCharacterById(model.Id_Character?? -1);
            if (characterModel == null)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new SessionModel()));
            }
            if (user.Id != characterModel.Id_User)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new SessionModel()));
            }
            _db.SaveSession(model);

            return Ok(ResponseHandler.GetAppResponse(type, model));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // DELETE 
    [HttpPost]
    [Route("DeleteSession")]
    public IActionResult Delete(SessionModel model)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            User user = _db.GetUserByToken(accessToken);
            if (user == null)
            {
                return StatusCode(501, "Token does not exist");
            }
            var isTokenWork = _tokenService.CheckTime(_db.GetUserByToken(accessToken));
            if (!isTokenWork)
            {
                return StatusCode(502, "Token does not work");
            }
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            CharacterModel characterModel = _db.GetCharacterById(model.Id_Character?? -1);
            if (characterModel == null)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, "Удаление не выполнено"));
            }
            if (user.Id != characterModel.Id_User)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, "Удаление не выполнено"));
            }
            
            _db.DeleteUserInSession(model.Id_Character?? -1);
            return Ok(ResponseHandler.GetAppResponse(type, "Удаление выполнено успешно."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    [HttpGet]
    [Route("GetSessionByIdTale/{id}")]
    public IActionResult GetSession(int id)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            User user = _db.GetUserByToken(accessToken);
            if (user == null)
            {
                return StatusCode(501, "Token does not exist");
            }
            var isTokenWork = _tokenService.CheckTime(_db.GetUserByToken(accessToken));
            if (!isTokenWork)
            {
                return StatusCode(502, "Token does not work");
            }
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            return Ok(ResponseHandler.GetAppResponse(type, _db.GetAllCharacterByIdTale(id)));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }
}