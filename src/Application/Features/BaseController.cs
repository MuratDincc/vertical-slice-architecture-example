using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Features;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    private ISender _mediator;
    
    protected ISender Mediator
    {
        get
        {
            if (_mediator == null)
                _mediator = HttpContext.RequestServices.GetRequiredService<ISender>();

            return _mediator;
        }
    }
}