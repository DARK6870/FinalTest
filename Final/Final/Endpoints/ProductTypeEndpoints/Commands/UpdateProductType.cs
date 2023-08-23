using Final.Endpoints.ProductTypeEndpoints.Models;
using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;


namespace Final.Endpoints.ProductTypeEndpoints.Commands
{
    [Route("api/productTypes")]
    [Authorize(Roles = "Manager")]
    public class UpdateProductType : EndpointBaseAsync
        .WithRequest<int, PTModel>
        .WithActionResult<PTModel>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public UpdateProductType(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPut("edit/{id}")]
        [SwaggerOperation(
        Summary = "Edit ProductType",
        Description = "This Api update information about ProductType with a specific Id",
        OperationId = "EditType",
        Tags = new[] { "ProductTypes Endpoint" })]
        public override async Task<ActionResult<PTModel>> HandleAsync(int id, [FromBody] PTModel model, CancellationToken cancellationToken)
        {
            var type = await _context.ProductTypes.FindAsync(id);
            if (type == null) return NotFound("ProductType with this id does not exist");

            bool ptexist = _context.ProductTypes.Any(p => p.TypeId.Equals(model.TypeId) && p.ProductId.Equals(model.ProductId));
            if (ptexist)
            {
                return BadRequest("This ProductType is already exist");
            }

            if (await _context.Products.FindAsync(model.ProductId) == null) return NotFound("Product with this id does not exist");
            if (await _context.Types.FindAsync(model.TypeId) == null) return NotFound("Type with this is does not exist");

            type.ProductId = model.ProductId;
            type.TypeId = model.TypeId;

            await _context.SaveChangesAsync();
            _cache.Remove("producttype_data");

            return Ok();
        }
    }
}
