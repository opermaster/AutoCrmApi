namespace AutoCrmApi.Models
{
    public class OrderService
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public int MasterId { get; set; }
        public User Master { get; set; } = null!;

        public bool Done { get; set; }

    }

}
