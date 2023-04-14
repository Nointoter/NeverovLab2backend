using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;
using NeverovLab2backend.Models.Auth;

namespace NeverovLab2backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : Controller
{
    private readonly DBHelper _db;

    public SessionsController(pgDbContext pgDbContext)
    {
        _db = new DBHelper(pgDbContext);
    }

    // POST 
    [HttpPost]
    [Route("SaveSession")]
    public IActionResult Post(SessionTokenModel model)// +id tale + id персонажи + token
    {
        ResponseType type = ResponseType.Success;
        try
        {
            User user = _db.GetUserByToken(model.token);
            CharacterModel characterModel = _db.GetCharacterById(model.sessionModel.Id_Character?? -1);
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
            _db.SaveSession(model.sessionModel);

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
    public IActionResult Delete(SessionTokenModel model)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            User user = _db.GetUserByToken(model.token);
            CharacterModel characterModel = _db.GetCharacterById(model.sessionModel.Id_Character?? -1);
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
            
            _db.DeleteCharacter(model.sessionModel.Id_Character?? -1);
            return Ok(ResponseHandler.GetAppResponse(type, "Удаление выполнено успешно."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    [HttpGet]
    [Route("GetSessionById/{id}")]
    public IActionResult GetSession(int id)//+id tale
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