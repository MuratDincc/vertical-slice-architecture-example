using Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customer;

[ApiExplorerSettings(GroupName = "Customer")]
public class CustomerUpdateController : BaseController
{
    [HttpPut("/api/customers/{id}")]
    public async Task<ActionResult> Update(int id, UpdateCustomerCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await Mediator.Send(command);

        return NoContent();
    }
}

public class UpdateCustomerCommand : IRequest
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
}

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        
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

internal sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateCustomerCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customer.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException(nameof(Entities.Customer), request.Id);

        entity.Name = request.Name;
        entity.Surname = request.Surname;

        if (entity.Email != request.Email)
        {
            if (await _context.Customer.AsNoTracking().AnyAsync(x => x.Email == request.Email, cancellationToken))
                throw new Exception("Email Registered");

            entity.Email = request.Email;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}