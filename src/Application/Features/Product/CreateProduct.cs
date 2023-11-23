using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Product;

[ApiExplorerSettings(GroupName = "Product")]
public class CreateProductController : BaseController
{
    [HttpPost("/api/products")]
    public async Task<ActionResult<int>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }
}

public record CreateProductCommand : IRequest<int>
{
    public string Title { get; init; }
    public decimal Price { get; init; }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entities.Product
        {
            Title = request.Title,
            Price = request.Price
        };

        _context.Product.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}