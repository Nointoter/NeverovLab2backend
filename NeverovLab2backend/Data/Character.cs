using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeverovLab2backend.Data;

[Table("Character")]
public class Character
{
    [Key, Required]
    public int? Id { get; set; }
    public int? Id_User { get; set; }
    public string? Name { get; set; }
    public int? Gender { get; set; }
    public string? Race { get; set; }
}
