using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;
using NeverovLab2backend.Services;


namespace NeverovLab2backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TalesController : Controller
{
    private readonly DBHelper _db;
    private readonly ITokenService _tokenService;
    public TalesController(pgDbContext pgDbContext)
    {
        _db = new DBHelper(pgDbContext);
    }
    // GET
    [HttpGet]
    [Route("GetTales")]
    public IActionResult GetAllTales()
    {
        ResponseType type = ResponseType.Success;
        try
        {            
            List<AllTaleInfoModel> taleModels = _db.GetAllTales();
            
            return Ok(ResponseHandler.GetAppResponse(type, taleModels));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    [HttpGet]
    [Route("GetTalesByIdTale/{id}")]
    public IActionResult GetTaleById(int id_tale)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            Tale tale = _db.GetTale(id_tale);
            if(tale==null)
            {
                return BadRequest(ResponseHandler.GetAppResponse(type, "Мастер пока не придумал такую историю!"));
            }
            return Ok(ResponseHandler.GetAppResponse(type, tale));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }
    [HttpGet]
    [Route("GetTalesByIdMaster/{id}")]
    public IActionResult GetByIdMaster(int id)
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
            TaleModel taleModel = _db.GetTaleByIdMaster(id);
            if (taleModel == null)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
            }
            if (user.Id != taleModel.Id_Master)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
            }
            return Ok(ResponseHandler.GetAppResponse(type, taleModel));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // POST api/<CharactersController>
    [HttpPost]
    [Route("SaveTale")]
    public IActionResult Post(TaleModel model)
    {
        try
        {
            /*var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            User user = _db.GetUserByToken(accessToken);
            model.Id_Master = user.Id;*/
            ResponseType type = ResponseType.Success;
            _db.SaveTale(model);
            return Ok(ResponseHandler.GetAppResponse(type, model));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // PUT api/<CharactersController>/5
    [HttpPut]
    [Route("UpdateTale")]
    public IActionResult Put(TaleModel model)
    {
        try
        {
            ResponseType type = ResponseType.Success;
            _db.SaveTale(model);
            return Ok(ResponseHandler.GetAppResponse(type, model));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // DELETE api/<CharactersController>/5
    [HttpDelete]
    [Route("DeleteTale/{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            ResponseType type = ResponseType.Success;
            _db.DeleteTale(id);
            _db.DeleteAllSession(id);
            return Ok(ResponseHandler.GetAppResponse(type, "Удаление выполнено успешно."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

}