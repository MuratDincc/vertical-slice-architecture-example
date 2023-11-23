namespace Application.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal Total { get; set; }
    
    public List<OrderItem> OrderItems { get; set; }
}
