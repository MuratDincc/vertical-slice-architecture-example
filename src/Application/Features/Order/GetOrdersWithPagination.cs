using Application.Features.Order.Dto;
using Application.PagedList;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Order;

[ApiExplorerSettings(GroupName = "Order")]
public class GetOrdersWithPaginationController : BaseController
{
    [HttpGet("/api/orders")]
    public Task<PaginationModel<OrderDto>> GetOrdersWithPagination([FromQuery] GetOrdersWithPaginationQuery query)
    {
        return Mediator.Send(query);
    }
}

public class GetOrdersWithPaginationQuery : IRequest<PaginationModel<OrderDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetOrdersWithPaginationQueryValidator : AbstractValidator<GetOrdersWithPaginationQuery>
{
    public GetOrdersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}

internal sealed class GetOrdersWithPaginationQueryHandler : IRequestHandler<GetOrdersWithPaginationQuery, PaginationModel<OrderDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOrdersWithPaginationQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationModel<OrderDto>> Handle(GetOrdersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Order
            .OrderBy(x => x.Id)
            .Select(x => new OrderDto
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                Total = x.Total,
                OrderItems = x.OrderItems.Select(y => new OrderDto.OrderItemDto
                {
                    Id = y.Id,
                    OrderId = y.OrderId,
                    ProductId = y.ProductId,
                    Title = y.Title,
                    Price = y.Price,
                    Quantity = y.Quantity,
                    Total = y.Total
                }).ToList()
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}