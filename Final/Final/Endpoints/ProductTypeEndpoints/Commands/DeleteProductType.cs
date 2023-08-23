using Final.Endpoints.ProductTypeEndpoints.Models;
using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.ProductTypeEndpoints.Commands
{
    [Route("api/productTypes")]
    [Authorize(Roles = "Manager")]
    public class DeleteProductType : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<ProductTypes>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public DeleteProductType(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpDelete("delete/{id}")]
        [SwaggerOperation(
        Summary = "Delete ProductType",
        Description = "This Api delete information about new ProductType with a specific Id",
        OperationId = "DeleteProductType",
        Tags = new[] { "ProductTypes Endpoint" })]
        public override async Task<ActionResult<ProductTypes>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var pt = await _context.ProductTypes.FindAsync(id);
            if (pt == null) return NotFound();

            _context.ProductTypes.Remove(pt);
            await _context.SaveChangesAsync();

            _cache.Remove("producttype_data");

            return Ok();
        }
    }
}
