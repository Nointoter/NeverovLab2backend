using System.ComponentModel.DataAnnotations;

namespace NeverovLab2backend.Models
{ 
    public class Member
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string login { get; set; }
        public string password { get; set; }

    }
}
