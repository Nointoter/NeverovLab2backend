using Microsoft.AspNetCore.Mvc;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;
using NeverovLab2backend.Models.Auth;
using NeverovLab2backend.Services;
using System.Web.Http;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            List<AllCharacterInfoModel> userCharModels = new List<AllCharacterInfoModel>();
            foreach(CharacterModel character in characterData)
            {
                userCharModels.Add(
                    new AllCharacterInfoModel()
                    {
                        Id = character.Id,
                        Id_User = character.Id_User,
                        Name_User = UserData.Where(d => d.Id.Equals(character.Id_User)).FirstOrDefault().Username,
                        Name =character.Name,
                        Gender= character.Gender,
                        Gender_Name = character.Gender switch
                        {
                            0 => "Неизвестно",
                            1 => "Мужчина",
                            2 => "Женщина",
                            3 => "Боевой вертолёт"
                        },
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
    public IActionResult Get(int id)
    {
        ResponseType type = ResponseType.Success;
        try
        {
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            var isTokenWork = _tokenService.CheckTime(_db.GetUserByToken(accessToken));
            User user = _db.GetUserByToken(accessToken);
            if (user == null)
            {
                return StatusCode(501, "Token does not exist");
            }
            if (!isTokenWork)
            {
                return StatusCode(502, "Token does not work");
            }
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            CharacterModel characterModel = _db.GetCharacterById(id);
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
    [HttpPost]
    [Route("SaveCharacter")]
    public IActionResult Post(CharacterModel model)
    {
        try
        {
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            var isTokenWork = _tokenService.CheckTime(_db.GetUserByToken(accessToken));
            User user = _db.GetUserByToken(accessToken);
            if (user == null)
            {
                return StatusCode(501, "Token does not exist");
            }
            if (!isTokenWork)
            {
                return StatusCode(502, "Token does not work");
            }
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            model.Id_User = user.Id;
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
    public IActionResult Put(CharacterModel model)
    {
        try
        {
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            var isTokenWork = _tokenService.CheckTime(_db.GetUserByToken(accessToken));
            User user = _db.GetUserByToken(accessToken);
            if (user == null)
            {
                return StatusCode(501, "Token does not exist");
            }
            if (!isTokenWork)
            {
                return StatusCode(502, "Token does not work");
            }
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }

            ResponseType type = ResponseType.Success;
           
            CharacterModel characterModel = _db.GetCharacterById(model.Id?? -1);
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
            var accessToken = Request.Headers[HeaderNames.Authorization][0].Remove(0, 7);
            var isOkToken = _tokenService.CheckToken(_db.GetUserByToken(accessToken));
            var isTokenWork = _tokenService.CheckTime(_db.GetUserByToken(accessToken));
            User user = _db.GetUserByToken(accessToken);
            if (user==null)
            {
                return StatusCode(501, "Token does not exist");
            }
            if(!isTokenWork)
            {
                return StatusCode(502, "Token does not work");
            }
            if (!isOkToken)
            {
                return StatusCode(401, "My error message");
            }
            
            ResponseType type = ResponseType.Success;
            
            CharacterModel characterModel = _db.GetCharacterById(id);
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
            _db.DeleteCharacter(id);
            return Ok(ResponseHandler.GetAppResponse(type, "Удаление выполнено успешно."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseHandler.GetExceptionResponse(ex));
        }
    }
}
