using AutoCrmApi.Models;

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
public class ServiceDto
{
    public int? Id { get; set; } = null!; 
    public string Name { get; set; } = null!;
    public bool? IsDone { get; set; } = null!;
    public decimal Price { get; set; }
    public UserDto? Master { get; set; } = null!;
    public int EstimatedTime { get; set; }

    public Service ToService() {
        return new Service {
            Name = Name,
            Price = Price,
            EstimatedTime = EstimatedTime,
        };
    }
}