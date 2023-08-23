using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

namespace Final.Endpoints.ProductEndpoints.Commands
{
    [Route("api/product")]
    [Authorize(Roles = "Manager")]
    public class DeleteProduct : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<Product>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public DeleteProduct(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpDelete("delete/{id}")]
        [SwaggerOperation(
        Summary = "Delete Product",
        Description = "This Api delete infromation about Product with a specific Id",
        OperationId = "DeleteProduct",
        Tags = new[] { "Product Endpoint" })]
        public override async Task<ActionResult<Product>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var res = await _context.Products.FindAsync(id);
            if (res == null) return NotFound();

            _context.Products.Remove(res);

            var relatedProductTypes = _context.ProductTypes.Where(pt => pt.ProductId == id);
            _context.ProductTypes.RemoveRange(relatedProductTypes);

            var relatedWishList = _context.WishLists.Where(wl => wl.ProductId == id);
            _context.WishLists.RemoveRange(relatedWishList);
            await _context.SaveChangesAsync();

            _cache.Remove("product_data");
			_cache.Remove("producttype_data");

			return Ok();
        }
    }
}
