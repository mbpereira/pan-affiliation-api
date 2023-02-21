using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly IValidationContext _context;

        public DefaultController(IValidationContext context)
        {
            _context = context;
        }

        protected ActionResult<GenericResponse<T?>> GenericResponse<T>(T? data = null) where T : class
            => new GenericResponseFactory<T>(data, _context).Create();
    }
}