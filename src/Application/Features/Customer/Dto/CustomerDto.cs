namespace Application.Features.Customer.Dto;

public record CustomerDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
}