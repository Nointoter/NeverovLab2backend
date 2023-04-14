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

    // POST api/<CharactersController>
    [HttpPost]
    [Route("SaveTale")]
    public IActionResult Post(SessionModel model)
    {
        try
        {
            ResponseType type = ResponseType.Success;
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