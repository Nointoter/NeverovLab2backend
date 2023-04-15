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
    public SessionsController(pgDbContext pgDbContext)
    {
        _db = new DBHelper(pgDbContext);
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
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            User user = _db.GetUserByToken(accessToken);
            CharacterModel characterModel = _db.GetCharacterById(model.Id_Character?? -1);
            if (characterModel == null)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
            }
            if (user.Id != characterModel.Id_User)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
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
    [HttpDelete]
    [Route("DeleteSession/{id}")]
    public IActionResult Delete(SessionModel model)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            User user = _db.GetUserByToken(accessToken);
            CharacterModel characterModel = _db.GetCharacterById(model.Id_Character?? -1);
            if (characterModel == null)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
            }
            if (user.Id != characterModel.Id_User)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
            }
            
            _db.DeleteCharacter(model.Id_Character?? -1);
            return Ok(ResponseHandler.GetAppResponse(type, "Удаление выполнено успешно."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    [HttpGet]
    [Route("GetSessionById/{id}")]
    public IActionResult GetSession(int id)
    {
        ResponseType type = ResponseType.Success;
        try
        {                    
            return Ok(ResponseHandler.GetAppResponse(type, _db.GetAllCharacterByIdTale(id)));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }
}