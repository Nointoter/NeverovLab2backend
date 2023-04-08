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
    [Route("api/[controller]/GetCharacters")]
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
}
