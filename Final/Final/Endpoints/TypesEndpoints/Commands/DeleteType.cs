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
    public class DeleteType : EndpointBaseAsync
        .WithRequest<byte>
        .WithActionResult<Types>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public DeleteType(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpDelete("delete/{id}")]
        [SwaggerOperation(
        Summary = "Delete Type",
        Description = "This Api delete information about Type with a specific Id",
        OperationId = "DeleteType",
        Tags = new[] { "Types Endpoint" })]
        public override async Task<ActionResult<Types>> HandleAsync(byte id, CancellationToken cancellationToken)
        {
            var type = await _context.Types.FindAsync(id);
            if (type == null) return NotFound();

            _context.Types.Remove(type);

            var relatedProductTypes = _context.ProductTypes.Where(pt => pt.TypeId == id);
            _context.ProductTypes.RemoveRange(relatedProductTypes);

            await _context.SaveChangesAsync();
            _cache.Remove("types_data");

            return Ok();
        }
    }
}
