using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeverovLab2backend.Data;

[Table("Session")]
public class Session
{
    [Key, Required]
    public int? Id_Tale { get; set; }
    public int? Id_Character { get; set; }
}