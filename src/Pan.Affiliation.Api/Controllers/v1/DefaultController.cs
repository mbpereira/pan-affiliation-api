using System.Net;
using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Domain.Shared;

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

        protected IActionResult GenericResponse(object? data = null)
            => new GenericResponseFactory(data, _context).Create();
    }
}