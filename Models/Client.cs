namespace AutoCrmApi.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public ICollection<Auto> Autos { get; set; } = new List<Auto>();
    }

}
