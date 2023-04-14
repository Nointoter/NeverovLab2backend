<<<<<<< Updated upstream
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
>>>>>>> Stashed changes
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;
using NeverovLab2backend.Models.Auth;
using NeverovLab2backend.Services;
using System.Net;
using System.Web.Http;

namespace NeverovLab2backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharactersController : Controller
{
    private readonly DBHelper _db;
    private readonly ITokenService _tokenService;

    public CharactersController(pgDbContext pgDbContext, ITokenService tokenService)
    {
        _db = new DBHelper(pgDbContext);
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
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
                        Id_User = character.Id_User,
                        NameMember = UserData.Where(d => d.Id.Equals(character.Id_User)).FirstOrDefault().Username,
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
    public IActionResult Get(IdUserTokenModel model)
    {
        ResponseType type = ResponseType.Success;
        try
        {   
            User user=_db.GetUserByToken(model.token);
            CharacterModel characterModel = _db.GetCharacterById(model.id??-1);
            if (characterModel == null)
            {
                type = ResponseType.NotFound;
                return Ok(ResponseHandler.GetAppResponse(type, new CharacterModel()));
            }
            if(user.Id != characterModel.Id_User)
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
    [Authorize]
    [HttpPost]
    [Route("SaveCharacter")]
    public IActionResult Post(CharacterTokenModel model)
    {
        try
        {
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7); ;
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            User user = _db.GetUserByToken(model.token);
            model.CharacterModel.Id_User = user.Id;
            ResponseType type = ResponseType.Success;
            _db.SaveCharacter(model.CharacterModel);
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
    public IActionResult Put(CharacterTokenModel model)
    {
        try
        {
            ResponseType type = ResponseType.Success;
            User user = _db.GetUserByToken(model.token);
            CharacterModel characterModel = _db.GetCharacterById(model.CharacterModel.Id?? -1);
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
            _db.SaveCharacter(model.CharacterModel);
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
    public IActionResult Delete(IdUserTokenModel model)
    {
        try
        {
            ResponseType type = ResponseType.Success;
            User user = _db.GetUserByToken(model.token);
            CharacterModel characterModel = _db.GetCharacterById(model.id ?? -1);
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
            _db.DeleteCharacter(model.id??-1);
            return Ok(ResponseHandler.GetAppResponse(type, "Удаление выполнено успешно."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }
}
