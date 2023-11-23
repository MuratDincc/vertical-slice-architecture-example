namespace Application.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public decimal Total { get; set; }
    
    public Order Order { get; set; }
    public Product Product { get; set; }
}
