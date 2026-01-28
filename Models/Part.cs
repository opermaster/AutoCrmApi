using AutoCrmApi.Models;

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
public class PartDto
{
    public int? Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int? Quantity { get; set; } = null!;
    public Part ToPart() {
        return new Part {
            Name = Name,
            Price =  Price,
    };
    }
}
