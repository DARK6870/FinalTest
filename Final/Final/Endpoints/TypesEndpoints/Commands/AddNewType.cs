using Final.Endpoints.ProductEndpoints.Commands;
using Final.Endpoints.ProductEndpoints.Models;
using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.TypesEndpoints.Commands
{
    [Route("api/types")]
    [Authorize(Roles = "Manager")]
    public class AddNewType : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult<Types>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public AddNewType(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPost("add")]
        [SwaggerOperation(
        Summary = "Add new Type",
        Description = "This Api add information about new Type in DB",
        OperationId = "AddNewType",
        Tags = new[] { "Types Endpoint" })]
        public override async Task<ActionResult<Types>> HandleAsync(string typeName, CancellationToken cancellationToken)
        {
            bool typeExist = _context.Types.Any(p => p.TypeName.Equals(typeName));
            if (typeExist)
            {
                return BadRequest("This type is already exist");
            }

            var type = new Types
            {
               TypeName = typeName
            };

            await _context.Types.AddAsync(type);
            await _context.SaveChangesAsync();
            _cache.Remove("types_data");

            return Ok();
        }
    }
}
