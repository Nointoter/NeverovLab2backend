using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

    public List<UserModel> GetAllUsers()
    {
        List<UserModel> response = new List<UserModel>();
        var dataList = _context.Users.ToList();
        dataList.ForEach(row => response.Add(new UserModel()
        {
            Id = row.Id,
            Username = row.Username          
        }));
        return response;
    }
    public User GetUserByRefreshToken(string refreshToken)
    {
        var row = _context.Users.Where(d => d.RefreshToken.Equals(refreshToken)).FirstOrDefault();
        return row;
    }
    
    public User GetUserByToken(string token)
    {
        var row = _context.Users.Where(d => d.Token.Equals(token)).FirstOrDefault();
        return row;
    }
    /// <summary>
    /// GET
    /// </summary>
    /// <returns></returns>
    public User GetUserByUserName(string username)
    {
        var row = _context.Users.Where(d => d.Username.Equals(username)).FirstOrDefault();
        return row;
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
            dbTable = _context.Users.Where(d => d.Id.Equals(userModel.Id)).FirstOrDefault();
            if (dbTable != null)
            {

                dbTable.Id = userModel.Id;
                dbTable.Username = userModel.Username;
                dbTable.PasswordHash = userModel.PasswordHash;
                dbTable.PasswordSalt = userModel.PasswordSalt;
                dbTable.Token = userModel.Token;
                dbTable.RefreshToken = userModel.RefreshToken;
                dbTable.TokenCreated = userModel.TokenCreated;
                dbTable.TokenExpires = userModel.TokenExpires;
                //_context.Users.Remove(dbTable);
                //_context.Users.Update(dbTable);
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
    /// <summary>
    /// POST/PUT
    /// </summary>
    /// <param name="taleModel"></param>
    public void SaveTale(TaleModel taleModel)
    {
        Tale dbTable = new Tale();

        if (taleModel.Id > 0)
        {
            //PUT
            dbTable = _context.Tales.Where(d => d.Id.Equals(taleModel.Id)).FirstOrDefault();
            if (dbTable != null)
            {

                dbTable.Id = taleModel.Id;
                dbTable.Name = taleModel.Name;
                dbTable.Id_Master = taleModel.Id_Master;
                dbTable.count_parties = taleModel.count_parties;
                dbTable.Start_Tale = taleModel.Start_Tale;
               
            }
        }
        else
        {
            //POST
            dbTable.Id = taleModel.Id;
            dbTable.Name = taleModel.Name;
            dbTable.Id_Master = taleModel.Id_Master;
            dbTable.count_parties = taleModel.count_parties;
            dbTable.Start_Tale = taleModel.Start_Tale;
            _context.Tales.Add(dbTable);
        }

        _context.SaveChanges();
    }
    //DELETE
    public void DeleteTale(int id)
    {
        var row = _context.Tales.Where(d => d.Id.Equals(id)).FirstOrDefault();
        if (row != null)
        {
            _context.Tales.Remove(row);
            _context.SaveChanges();
        }
    }
    public List<TaleModel> GetTaleByIdMaster(int id_master)
    {
        List<TaleModel> taleModel = new List<TaleModel>();
        var dataList = _context.Tales.ToList();       
        foreach (var row in dataList)
        {
            if(row.Id_Master ==id_master)
            {
                taleModel.Add(new TaleModel()
                {
                    Id = row.Id,
                    Name = row.Name,
                    Id_Master = row.Id_Master,
                    count_parties = row.count_parties,
                    Start_Tale = row.Start_Tale
                });
            }
        }
        return taleModel;
    }

    public List<TaleModel> GetAllTales()
    {
        List<TaleModel> response = new List<TaleModel>();
        var dataList = _context.Tales.ToList();
        dataList.ForEach(row => response.Add(new TaleModel()
        {
            Id = row.Id,
            Name = row.Name,
            Id_Master = row.Id_Master,
            count_parties = row.count_parties,
            Start_Tale = row.Start_Tale
        }));
        return response;
    }
}