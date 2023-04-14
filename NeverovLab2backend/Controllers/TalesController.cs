using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;
using NeverovLab2backend.Models.Auth;


namespace NeverovLab2backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TalesController : Controller
{
    private readonly DBHelper _db;

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
            List<TaleModel> taleModels = _db.GetAllTales();
            
            return Ok(ResponseHandler.GetAppResponse(type, taleModels));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    [HttpGet]
    [Route("GetTalesByIdMaster/{id}")]
    public IActionResult GetByIdMaster(IdMemberTokenModel model)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            User user = _db.GetUserByToken(model.token);
            TaleModel taleModel = _db.GetTaleByIdMaster(model.id ?? -1);
            if (taleModel == null)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
            }
            if (user.Id != taleModel.Id)
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
    public IActionResult Post(TaleTokenModel model)
    {
        try
        {
            User user = _db.GetUserByToken(model.token);
            model.taleModel.Id_Master = user.Id;
            ResponseType type = ResponseType.Success;
            _db.SaveTale(model.taleModel);
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
            return Ok(ResponseHandler.GetAppResponse(type, "Delete Successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

}