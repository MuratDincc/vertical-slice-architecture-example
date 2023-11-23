using Application.Features.Customer.Dto;
using Application.PagedList;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Customer;

[ApiExplorerSettings(GroupName = "Customer")]
public class GetCustomersWithPaginationController : BaseController
{
    [HttpGet("/api/customers")]
    public Task<PaginationModel<CustomerDto>> GetCustomersWithPagination([FromQuery] GetCustomersWithPaginationQuery query)
    {
        return Mediator.Send(query);
    }
}

public class GetCustomersWithPaginationQuery : IRequest<PaginationModel<CustomerDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetCustomersWithPaginationQueryValidator : AbstractValidator<GetCustomersWithPaginationQuery>
{
    public GetCustomersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}

internal sealed class GetCustomersWithPaginationQueryHandler : IRequestHandler<GetCustomersWithPaginationQuery, PaginationModel<CustomerDto>>
{
    private readonly ApplicationDbContext _context;

    public GetCustomersWithPaginationQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationModel<CustomerDto>> Handle(GetCustomersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customer
            .OrderBy(x => x.Id)
            .Select(x => new CustomerDto
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}