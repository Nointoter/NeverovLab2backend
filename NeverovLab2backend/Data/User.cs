using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeverovLab2backend.Data;

[Table("User")]
public class User
{
    [Key, Required]
    public int? Id { get; set; }
    public string? Username { get; set; } = string.Empty;
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string? Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public string? TokenCreated { get; set; }
    public string? TokenExpires { get; set; }
}
