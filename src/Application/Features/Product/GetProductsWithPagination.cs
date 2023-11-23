using Application.Features.Product.Dto;
using Application.PagedList;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Product;

[ApiExplorerSettings(GroupName = "Product")]
public class GetProductsWithPaginationController : BaseController
{
    [HttpGet("/api/products")]
    public Task<PaginationModel<ProductDto>> GetProdutsWithPagination([FromQuery] GetProductsWithPaginationQuery query)
    {
        return Mediator.Send(query);
    }
}

public class GetProductsWithPaginationQuery : IRequest<PaginationModel<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetProductsWithPaginationQueryValidator : AbstractValidator<GetProductsWithPaginationQuery>
{
    public GetProductsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}

internal sealed class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, PaginationModel<ProductDto>>
{
    private readonly ApplicationDbContext _context;

    public GetProductsWithPaginationQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationModel<ProductDto>> Handle(GetProductsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Product
            .OrderBy(x => x.Title)
            .Select(x => new ProductDto
            {
                Title = x.Title,
                Price = x.Price
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}