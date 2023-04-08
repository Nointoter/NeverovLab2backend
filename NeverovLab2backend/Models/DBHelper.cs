using NeverovLab2backend.Data;
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