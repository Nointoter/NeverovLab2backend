namespace NeverovLab2backend.Models
{
    public class Tale
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Id_Master { get; set; }
        public int count_parties { get; set; }
        public DateTime Start_Tale { get; set; }

    }
}