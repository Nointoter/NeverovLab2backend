using Microsoft.AspNetCore.Mvc;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;

namespace NeverovLab2backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharactersController : Controller
{
    private readonly DBHelper _db;
    public CharactersController(pgDbContext pgDbContext)
    {
        _db = new DBHelper(pgDbContext);
    }
    // GET: api/<CharactersController>
    [HttpGet]
    [Route("GetCharacters")]
    public IActionResult Get()
    {
        ResponseType type = ResponseType.Success;
        try
        {
            IEnumerable<CharacterModel> data = _db.GetCharacters();
            if (!data.Any())
            {
                type = ResponseType.NotFound;
            }
            return Ok(ResponseHandler.GetAppResponse(type, data));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // GET api/<CharactersController>/5
    [HttpGet]
    [Route("GetCharacterById/{id}")]
    public IActionResult Get(int id)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            CharacterModel data = _db.GetCharacterById(id);

            if (data == null)
            {
                type = ResponseType.NotFound;
            }
            return Ok(ResponseHandler.GetAppResponse(type, data));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // POST api/<CharactersController>
    [HttpPost]
    [Route("SaveCharacter")]
    public IActionResult Post([FromBody] CharacterModel model)
    {
        try
        {
            ResponseType type = ResponseType.Success;
            _db.SaveCharacter(model);
            return Ok(ResponseHandler.GetAppResponse(type, model));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // PUT api/<CharactersController>/5
    [HttpPut]
    [Route("UpdateCharacter")]
    public IActionResult Put([FromBody] CharacterModel model)
    {
        try
        {
            ResponseType type = ResponseType.Success;
            _db.SaveCharacter(model);
            return Ok(ResponseHandler.GetAppResponse(type, model));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // DELETE api/<CharactersController>/5
    [HttpDelete]
    [Route("DeleteCharacter/{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            ResponseType type = ResponseType.Success;
            _db.DeleteCharacter(id);
            return Ok(ResponseHandler.GetAppResponse(type, "Delete Successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }
}
