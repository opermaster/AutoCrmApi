namespace AutoCrmApi.Models
{
    public class Auto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }

        public string VIN { get; set; } = null!;
        public string Number { get; set; } = null!;

        public Client Client { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
