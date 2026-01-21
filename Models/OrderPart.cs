namespace AutoCrmApi.Models
{
    public class OrderPart
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int PartId { get; set; }
        public Part Part { get; set; } = null!;

        public int Quantity { get; set; }
    }

}
