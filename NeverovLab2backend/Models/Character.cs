using System.ComponentModel.DataAnnotations;

namespace NeverovLab2backend.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public int Id_Member { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }

        
    }
}