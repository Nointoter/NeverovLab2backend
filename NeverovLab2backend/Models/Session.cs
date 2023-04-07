using System.ComponentModel.DataAnnotations;

namespace NeverovLab2backend.Models
{
    public class Session
    {
        [Key]
        public int Id_Tale { get; set; }
        public int Id_Character { get; set; }
    }
}
