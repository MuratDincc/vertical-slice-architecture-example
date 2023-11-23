namespace Application.Features.Product.Dto;

public record ProductDto
{
    public string Title { get; init; }
    public decimal Price { get; init; }
}