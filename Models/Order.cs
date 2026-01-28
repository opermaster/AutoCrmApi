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
    public class OrderDto
    {

        public DateTime  CreatedAt { get; set; }
        public DateTime?  CompletedAt { get; set; }
        public OrderStatus Status { get; set; }
        public string      Number { get; set; }
        public decimal     TotalCost { get; set; }
        public string?    Comment { get; set; }
        public Order ToOrder() {
            return new Order {
                CreatedAt = DateTime.SpecifyKind(CreatedAt, DateTimeKind.Utc),
                CompletedAt = CompletedAt.HasValue ? DateTime.SpecifyKind(CompletedAt.Value, DateTimeKind.Utc)
                    : null,
                Status = Status,
                TotalCost = TotalCost,
                Comment = Comment,
            };
        }
    }
    
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public OrderStatus Status { get; set; }

        public AutoDto Auto { get; set; } = null!;

        public decimal TotalCost { get; set; }
        public string? Comment { get; set; }

        public List<ServiceDto> Services { get; set; } = new();
        public List<PartDto> Parts { get; set; } = new();
        public static OrderResponseDto MapToDto(
                Order order,
                int? masterId,
                bool isAdmin
            ) {
            return new OrderResponseDto {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                CompletedAt = order.CompletedAt,
                Status = order.Status,

                Auto = new AutoDto {
                    Brand = order.Auto.Brand,
                    Model = order.Auto.Model,
                    Year = order.Auto.Year,
                    VIN = order.Auto.VIN,
                    Number = order.Auto.Number
                },

                TotalCost = order.TotalCost,
                Comment = order.Comment,

                Services = order.Services
                    .Where(os => isAdmin || os.MasterId == masterId)
                    .Select(os => new ServiceDto {
                        Id = os.Service.Id,
                        Name = os.Service.Name,
                        Price = os.Service.Price,
                        EstimatedTime = os.Service.EstimatedTime,
                        IsDone = os.Done,
                        Master = new UserDto {
                            Id = os.Master.Id,
                            Login = os.Master.Login
                        }
                    })
                    .ToList(),

                Parts = order.Parts.Select(op => new PartDto {
                    Id = op.Part.Id,
                    Name = op.Part.Name,
                    Price = op.Part.Price,
                    Quantity = op.Quantity
                }).ToList()
            };
        }


    }
    public class OrderUpdateDto {
        public class Pair {
            public int ServiceId { get; set; }
            public bool Done { get; set; }
        }
        public OrderStatus Status { get; set;}
        public int OrderId { get; set; }
        public string Comment { get; set; } = null!;
        public List<Pair> ServicesDone { get; set; } = new();
    }
}

