namespace AutoCrmApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public OrderStatus Status { get; set; }

        public int AutoId { get; set; }
        public Auto Auto { get; set; } = null!;

        public decimal TotalCost { get; set; }
        public string? Comment { get; set; }

        public ICollection<OrderService> Services { get; set; } = new List<OrderService>();
        public ICollection<OrderPart> Parts { get; set; } = new List<OrderPart>();
    }
}

