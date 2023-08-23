using Final.Endpoints.ProductEndpoints.Commands;
using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.TypesEndpoints.Commands
{
    [Route("api/types")]
    [Authorize(Roles = "Manager")]
    public class EditType : EndpointBaseAsync
        .WithRequest<byte, string>
        .WithActionResult<Types>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public EditType(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPut("edit/{id}")]
        [SwaggerOperation(
        Summary = "Edit Type",
        Description = "This Api update information about Type with a specific Id",
        OperationId = "EditType",
        Tags = new[] { "Types Endpoint" })]
        public override async Task<ActionResult<Types>> HandleAsync(byte id, string typeName, CancellationToken cancellationToken)
        {
            var type = await _context.Types.FindAsync(id);
            if (type == null) return NotFound();

            bool typeExist = _context.Types.Any(p => p.TypeName.Equals(typeName));
            if (typeExist)
            {
                return BadRequest("This type is already exist");
            }

            type.TypeName = typeName;

            await _context.SaveChangesAsync();
            _cache.Remove("types_data");

            return Ok();
        }
    }
}
