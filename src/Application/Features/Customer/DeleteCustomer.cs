using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customer;

[ApiExplorerSettings(GroupName = "Customer")]
public class DeleteCustomerController : BaseController
{
    [HttpDelete("/api/customers/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCustomerCommand { Id = id });

        return NoContent();
    }
}

public class DeleteCustomerCommand : IRequest
{
    public int Id { get; set; }
}

internal sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteCustomerCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customer.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException(nameof(Entities.Customer), request.Id);

        _context.Customer.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}