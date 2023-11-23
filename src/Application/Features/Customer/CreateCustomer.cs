using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Customer;

[ApiExplorerSettings(GroupName = "Customer")]
public class CreateCustomerController : BaseController
{
    [HttpPost("/api/customers")]
    public async Task<IActionResult> Create(CreateCustomerCommand command)
    {
        await Mediator.Send(command);
        
        return NoContent();
    }
}

public record CreateCustomerCommand : IRequest<int>
{
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
}

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(x => x.Surname)
            .MaximumLength(200)
            .NotEmpty();
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
    }
}

internal sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateCustomerCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entities.Customer
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email
        };

        _context.Customer.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}