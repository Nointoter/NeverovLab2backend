using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeverovLab2backend.Data;

[Table("Member")]
public class Member
{
    [Key, Required]
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? login { get; set; }
    public string? password { get; set; }
    
}