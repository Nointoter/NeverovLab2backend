﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;
using NeverovLab2backend.Models.Auth;

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

            IEnumerable<CharacterModel> characterData = _db.GetCharacters();
            if (!characterData.Any())
            {
                type = ResponseType.NotFound;
            }
            IEnumerable<UserModel> UserData = _db.GetAllUsers();
            if (!UserData.Any())
            {
                type = ResponseType.NotFound;
            }
            List<UserCharModel> userCharModels = new List<UserCharModel>();
            foreach(CharacterModel character in characterData)
            {
                userCharModels.Add(
                    new UserCharModel()
                    {
                        Id = character.Id,
                        Id_Member = character.Id_Member,
                        NameMember = UserData.Where(d => d.Id.Equals(character.Id)).FirstOrDefault().Username,
                        Name =character.Name,
                        Gender= character.Gender,
                        Race = character.Race
                    }
                    ) ;
            }
            return Ok(ResponseHandler.GetAppResponse(type, userCharModels));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }

    // GET api/<CharactersController>/5
    [HttpGet]
    [Route("GetCharacterById/{id}")]
    public IActionResult Get(int id, string token)
    {
        ResponseType type = ResponseType.Success;
        try
        {   
            User user=_db.GetUserByToken(token);
            CharacterModel characterModel = _db.GetCharacterById(id);
            if (characterModel == null)
            {
                type = ResponseType.NotFound;
            }
            if(user.Id != characterModel.Id)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
            }
            return Ok(ResponseHandler.GetAppResponse(type, characterModel));
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
