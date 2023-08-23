using Final.Endpoints.ProductEndpoints.Models;
using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.ProductEndpoints.Commands
{
    [Route("api/product")]
    [Authorize(Roles = "Manager")]
    public class UpdateProduct : EndpointBaseAsync
        .WithRequest<int, ProductModel>
        .WithActionResult<ProductModel>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public UpdateProduct(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPut("update")]
        [SwaggerOperation(
        Summary = "Update Product information",
        Description = "This Api update information about product with a specific Id",
        OperationId = "UpdateProduct",
        Tags = new[] { "Product Endpoint" })]
        public override async Task<ActionResult<ProductModel>> HandleAsync(int id, [FromBody] ProductModel updatedProduct, CancellationToken cancellationToken)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null) return NotFound();

            bool productExists = _context.Products.Any(p => p.ProductName.Equals(updatedProduct.ProductName));
            if (productExists)
            {
                return BadRequest("This Product is already exist");
            }

            existingProduct.ProductName = updatedProduct.ProductName;
            existingProduct.ImagePath = updatedProduct.ImagePath;
            existingProduct.price = updatedProduct.price;

            await _context.SaveChangesAsync();
            _cache.Remove("product_data");
            return Ok();
        }
    }
}
