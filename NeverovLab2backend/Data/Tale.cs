using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeverovLab2backend.Data;

[Table("Tale")]
public class Tale
{
    [Key, Required]
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? Id_Master { get; set; }
    public int? count_parties { get; set; }
    public string? Start_Tale { get; set; }
}