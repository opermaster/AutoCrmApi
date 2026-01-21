namespace AutoCrmApi.Models
{
    public class Service
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int EstimatedTime { get; set; }

        public ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
    }

}
