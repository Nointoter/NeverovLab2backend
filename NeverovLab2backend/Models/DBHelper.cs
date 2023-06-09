﻿using Microsoft.AspNetCore.Identity;
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
    public bool CheackUserName(string login)
    {
        var row = _context.Users.Where(d => d.Username.Equals(login)).FirstOrDefault();
        if (row == null)
            return true;
        return false;
        
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

    public User GetUserByUserId(int id)
    {
        var row = _context.Users.Where(d => d.Id.Equals(id)).FirstOrDefault();
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
            Id_User = row.Id_User,
            Name = row.Name,
            Race = row.Race,    
            Gender = row.Gender,
            
        }));
        return response;
    }
    public CharacterModel GetCharacterById(int id)
    {
        CharacterModel response = new CharacterModel();
        if (id == -1)
            return response;
        var row = _context.Characters.Where(d => d.Id.Equals(id)).FirstOrDefault();
        return new CharacterModel()
        {
            Id = row.Id,
            Id_User = row.Id_User,
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
                dbTable.Id_User = characterModel.Id_User;
                dbTable.Name = characterModel.Name;
                dbTable.Race = characterModel.Race;
                dbTable.Gender = characterModel.Gender;
            }
        }
        else
        {
            //POST
            dbTable.Id_User = characterModel.Id_User;
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
        if (id == -1)
            return;
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

                
                dbTable.Name = taleModel.Name;
                dbTable.Id_Master = taleModel.Id_Master;
                dbTable.count_parties = taleModel.count_parties;
                
               
            }
        }
        else
        {
            //POST
           
            dbTable.Name = taleModel.Name;
            dbTable.Id_Master = taleModel.Id_Master;
            dbTable.count_parties = taleModel.count_parties;
            dbTable.Start_Tale = Convert.ToString(DateTime.Now);
            _context.Tales.Add(dbTable);
        }

        _context.SaveChanges();
    }
    public Tale GetTale(int id)
    {
        Tale row = _context.Tales.Where(d => d.Id.Equals(id)).FirstOrDefault();
        if (row != null)
        {
            return(row);
        }
        return (null);
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
    public List<TaleModel> GetTalesByIdMaster(int id_master)
    {
        List<TaleModel> response = new List<TaleModel>();
        if (id_master == -1)
            return response; 
        var rows = _context.Tales.Where(d => d.Id_Master.Equals(id_master)).ToList();
        
        foreach ( var row in rows )
        {
            response.Add(new TaleModel()
            {
                Id = row.Id,
                Name = row.Name,
                Id_Master = row.Id_Master,
                count_parties = row.count_parties,
                Start_Tale = row.Start_Tale
            });
        }
        return response;
        
    }

    public List<AllTaleInfoModel> GetAllTales()
    {
        List<AllTaleInfoModel> response = new List<AllTaleInfoModel>();
        var dataList = _context.Tales.ToList();
        
        foreach(var row in dataList)
        {
            AllTaleInfoModel model = new AllTaleInfoModel();
            model.Id = row.Id;
            model.Name = row.Name;
            model.Id_Master = row.Id_Master;
            model.Name_Master = GetUserByUserId(row.Id_Master ?? -1).Username;
            model.count_parties = row.count_parties;
            model.Start_Tale = row.Start_Tale;
            response.Add(model);
        }
        return response;
    }

    public List<AllCharacterInfoModel> GetAllCharactersInfo(int id)
    {
        List<AllCharacterInfoModel> response = new List<AllCharacterInfoModel>();
        var dataList = _context.Sessions.ToList();
        var character = GetCharacterById(id);
        foreach (var row in dataList)
        {
            if (row.Id_Character == id)
            {
                response.Add(new AllCharacterInfoModel()
                {
                    Id = character.Id,
                    Id_User = character.Id_User,
                    Name_User = GetUserByUserId(id).Username,
                    Name = character.Name,
                    Gender = character.Gender,
                    Gender_Name = character.Gender switch
                    {
                        0 => "Неизвестно",
                        1 => "Мужчина",
                        2 => "Женщина",
                        3 => "Боевой вертолёт"
                    },
                    Race = character.Race,
                    Name_Tale = GetTale(row.Id_Tale ?? -1).Name
                });
            }
        }
        return response;
    }
    /// <summary>
    /// POST
    /// </summary>
    /// <param name="sessionModel"></param>
    public void SaveSession(SessionModel sessionModel)
    {
        Session dbTable = new Session();

        //POST
        dbTable.Id_Tale=sessionModel.Id_Tale;
        dbTable.Id_Character = sessionModel.Id_Character;
        
        _context.Sessions.Add(dbTable);
        
        _context.SaveChanges();
    }

    public void DeleteUserInSession(int id)
    {
        var row = _context.Sessions.Where(d => d.Id_Character.Equals(id)).FirstOrDefault();
        if (row != null)
        {
            _context.Sessions.Remove(row);
            _context.SaveChanges();
        }
    }
    /// <summary>
    /// Вывод игроков на экран
    /// </summary>
    /// <param name="id_tale"></param>
    /// <returns></returns>
    
    
    public void DeleteAllSession(int id)
    {
        
        var rows = _context.Sessions.Where(d => d.Id_Tale.Equals(id)).ToList();
        foreach (var row in rows)
        { 
            _context.Sessions.Remove(row);                   
        }
        _context.SaveChanges();
    }
    public List<CharacterModel> GetAllCharacterByIdTale(int id_tale)
    {
        List<CharacterModel> CharacterModel = new List<CharacterModel>();
        var dataList = _context.Sessions.ToList();
        foreach (var row in dataList)
        {
            if (row.Id_Tale == id_tale)
            {
                var character = _context.Characters.Where(d => d.Id.Equals(row.Id_Character)).FirstOrDefault();
                CharacterModel.Add(new CharacterModel()
                {
                    Id = character.Id,
                    Name = character.Name,
                    Race = character.Race
                }) ;
            }
        }
        return CharacterModel;
    }
}