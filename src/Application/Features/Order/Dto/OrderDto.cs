namespace Application.Features.Order.Dto;

public record OrderDto
{
    public record OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
    }
    
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public decimal Total { get; init; }
    
    public List<OrderItemDto> OrderItems { get; init; }
}