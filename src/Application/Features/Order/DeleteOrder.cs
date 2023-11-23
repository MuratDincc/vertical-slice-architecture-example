using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Order;

[ApiExplorerSettings(GroupName = "Order")]
public class DeleteOrderController : BaseController
{
    [HttpDelete("/api/orders/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteOrderCommand { Id = id });

        return NoContent();
    }
}

public class DeleteOrderCommand : IRequest
{
    public int Id { get; set; }
}

internal sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteOrderCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Order.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException(nameof(Entities.Order), request.Id);

        _context.Order.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}