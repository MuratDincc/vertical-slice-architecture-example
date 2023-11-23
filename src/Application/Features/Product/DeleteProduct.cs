using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Product;

[ApiExplorerSettings(GroupName = "Product")]
public class DeleteProductController : BaseController
{
    [HttpDelete("/api/products/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProductCommand { Id = id });

        return NoContent();
    }
}

public class DeleteProductCommand : IRequest
{
    public int Id { get; set; }
}

internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Product.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException(nameof(Entities.Product), request.Id);

        _context.Product.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}