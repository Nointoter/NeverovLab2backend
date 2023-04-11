using Microsoft.AspNetCore.Identity;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models.Auth;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

namespace NeverovLab2backend.Models;

public class DBHelper
{
    private pgDbContext _context;
    public DBHelper(pgDbContext context)
    {
        _context = context;
    }
    public List<SessionModel> GetSession()
    {
        List<SessionModel> response = new List<SessionModel>();
        var dataList = _context.Sessions.ToList();
        dataList.ForEach(row => response.Add(new SessionModel()
        {
            Id_Tale = row.Id_Tale,
            Id_Character = row.Id_Character
           
        }));
        return response;
    }

    public void DeleteSessionCharacter(int id)
    {
        var order = _context.Sessions.Where(d => d.Id_Character.Equals(id)).FirstOrDefault();
        if (order != null)
        {
            _context.Sessions.Remove(order);
            _context.SaveChanges();
        }
    }

    public UserModel GetUserByToken(string token)
    {
        var row = _context.Users.Where(d => d.Token.Equals(token)).FirstOrDefault();
        return new UserModel()
        {
            Id = row.Id,
            Username = row.Username,
            PasswordHash = row.PasswordHash,
            PasswordSalt = row.PasswordSalt,
            Token = row.Token,
            RefreshToken = row.RefreshToken,
            TokenCreated = row.TokenCreated,
            TokenExpires = row.TokenExpires,
        };
    }
    /// <summary>
    /// GET
    /// </summary>
    /// <returns></returns>
    public UserModel GetUserByUserName(string username)
    {
        var row = _context.Users.Where(d => d.Username.Equals(username)).FirstOrDefault();
        return new UserModel()
        {
            Id= row.Id,
            Username = row.Username,
            PasswordHash= row.PasswordHash,
            PasswordSalt= row.PasswordSalt,
            Token = row.Token,
            RefreshToken = row.RefreshToken,
            TokenCreated= row.TokenCreated,
            TokenExpires= row.TokenExpires,
        };
    }
    /// <summary>
    /// It serves the POST/PUT/PATCH
    /// </summary>
    public void SaveUser(UserModel userModel)
    {
        User dbTable = new User();

        if (userModel.Id > 0)
        {
            //PUT
            var row = _context.Users.Where(d => d.Username.Equals(userModel.Id)).FirstOrDefault();
            if (dbTable != null)
            {
                dbTable.Username = row.Username;
                dbTable.PasswordHash = row.PasswordHash;
                dbTable.PasswordSalt = row.PasswordSalt;
                dbTable.Token = row.Token;
                dbTable.RefreshToken = row.RefreshToken;
                dbTable.TokenCreated = row.TokenCreated;
                dbTable.TokenExpires = row.TokenExpires;
            }
        }
        else
        {
            //POST
            dbTable.Username = userModel.Username;
            dbTable.PasswordHash = userModel.PasswordHash;
            dbTable.PasswordSalt = userModel.PasswordSalt;
            dbTable.Token = userModel.Token;
            dbTable.RefreshToken = userModel.RefreshToken;
            dbTable.TokenCreated = userModel.TokenCreated;
            dbTable.TokenExpires = userModel.TokenExpires;
            _context.Users.Add(dbTable);
        }

        _context.SaveChanges();
    }

    /// <summary>
    /// GET
    /// </summary>
    /// <returns></returns>
    public List<CharacterModel> GetCharacters()
    {
        List<CharacterModel> response = new List<CharacterModel>();
        var dataList = _context.Characters.ToList();
        dataList.ForEach(row => response.Add(new CharacterModel()
        {
            Id = row.Id,
            Id_Member = row.Id_Member,
            Name = row.Name,
            Race = row.Race,
            Gender = row.Gender,
            
        }));
        return response;
    }
    public CharacterModel GetCharacterById(int id)
    {
        CharacterModel response = new CharacterModel();
        var row = _context.Characters.Where(d => d.Id.Equals(id)).FirstOrDefault();
        return new CharacterModel()
        {
            Id = row.Id,
            Id_Member = row.Id_Member,
            Name = row.Name,
            Race = row.Race,
            Gender = row.Gender,
        };
    }
    /// <summary>
    /// It serves the POST/PUT/PATCH
    /// </summary>
    public void SaveCharacter(CharacterModel characterModel)
    {
        Character dbTable = new Character();
        if (characterModel.Id > 0)
        {
            //PUT
            dbTable = _context.Characters.Where(d => d.Id.Equals(characterModel.Id)).FirstOrDefault();
            if (dbTable != null)
            {
                dbTable.Id_Member = characterModel.Id_Member;
                dbTable.Name = characterModel.Name;
                dbTable.Race = characterModel.Race;
                dbTable.Gender = characterModel.Gender;
            }
        }
        else
        {
            //POST
            dbTable.Id_Member = characterModel.Id_Member;
            dbTable.Name = characterModel.Name;
            dbTable.Race = characterModel.Race;
            dbTable.Gender = characterModel.Gender;
            _context.Characters.Add(dbTable);
        }
        _context.SaveChanges();
    }
    /// <summary>
    /// DELETE
    /// </summary>
    /// <param name="id"></param>
    public void DeleteCharacter(int id)
    {
        var order = _context.Characters.Where(d => d.Id.Equals(id)).FirstOrDefault();
        if (order != null)
        {
            _context.Characters.Remove(order);
            _context.SaveChanges();
        }
    }
}