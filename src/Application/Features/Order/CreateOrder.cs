using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Order;

[ApiExplorerSettings(GroupName = "Order")]
public class CreateOrderController : BaseController
{
    [HttpPost("/api/orders")]
    public async Task<ActionResult<int>> Create(CreateOrderCommand command)
    {
        return await Mediator.Send(command);
    }
}

public record CreateOrderCommand : IRequest<int>
{
    public record OrderItem
    {
        public int OrderId { get; init; }
        public int ProductId { get; init; }
        public string Title { get; init; }
        public decimal Price { get; init; }
        public decimal Quantity { get; init; }
        public decimal Total { get; init; }
    }
    
    public int CustomerId { get; init; }
    public decimal Total { get; init; }
    
    public List<OrderItem> OrderItems { get; init; }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.Total)
            .GreaterThan(0);
    }
}

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateOrderCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entities.Order
        {
            CustomerId = request.CustomerId,
            Total = request.Total,
            OrderItems = request.OrderItems.Select(x => new Entities.OrderItem
            {
                ProductId = x.ProductId,
                Title = x.Title,
                Price = x.Price,
                Quantity = x.Quantity,
                Total = x.Total
            }).ToList()
        };

        _context.Order.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}