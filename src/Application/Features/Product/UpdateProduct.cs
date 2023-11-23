using Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Product;

[ApiExplorerSettings(GroupName = "Product")]
public class ProductUpdateController : BaseController
{
    [HttpPut("/api/products/{id}")]
    public async Task<ActionResult> Update(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await Mediator.Send(command);

        return NoContent();
    }
}

public class UpdateProductCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(250)
            .NotEmpty();

        RuleFor(v => v.Price)
            .GreaterThan(0);
    }
}

internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Product.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException(nameof(Entities.Product), request.Id);

        entity.Title = request.Title;
        entity.Price = request.Price;

        await _context.SaveChangesAsync(cancellationToken);
    }
}