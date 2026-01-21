namespace AutoCrmApi.Models
{
    public class Part
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        public ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>();
    }

}
