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
    public TalesController(pgDbContext pgDbContext, ITokenService tokenService)
    {
        _db = new DBHelper(pgDbContext);
        _tokenService = tokenService;
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
    public IActionResult GetTaleById(int id)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            Tale tale = _db.GetTale(id);
            if(tale==null)
            {
                return BadRequest(ResponseHandler.GetAppResponse(type, "������ ���� �� �������� ����� �������!"));
            }
            return Ok(ResponseHandler.GetAppResponse(type, tale));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }
    [HttpGet]
    [Route("GetTalesByIdMaster")]
    public IActionResult GetByIdMaster()
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

            var taleModel = _db.GetTalesByIdMaster(user.Id ?? -1);
            if (taleModel == null || taleModel.Count == 0)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new List<TaleModel>()));
            }
            if (user.Id != taleModel[0].Id_Master)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new List<TaleModel>()));
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
            model.Id_Master = user.Id;
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
            ResponseType type = ResponseType.Success;
            _db.DeleteTale(id);
            _db.DeleteAllSession(id);
            return Ok(ResponseHandler.GetAppResponse(type, "�������� ��������� �������."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

}