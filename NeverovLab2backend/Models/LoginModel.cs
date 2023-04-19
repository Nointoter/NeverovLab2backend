using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeverovLab2backend.Models;

public class LoginModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? RefreshToken { get; set; }
    public string RefreshTokenExpiryTime { get; set; }//dataType
}
