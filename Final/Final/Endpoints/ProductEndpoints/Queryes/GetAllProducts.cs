using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Final.Endpoints.ProductEndpoints.Queryes
{
    [Route("api/product")]
    public class GetAllProducts : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<Product>>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public GetAllProducts(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        [SwaggerOperation(
        Summary = "Get all Products",
        Description = "This Api return full list of Products",
        OperationId = "GetAllProducts",
        Tags = new[] { "Product Endpoint" })]
        public override async Task<ActionResult<IEnumerable<Product>>> HandleAsync(CancellationToken cancellationToken)
        {
            var res = await _cache.GetOrAddAsync("product_data", async () =>
            {
                var result = await _context.Products.ToListAsync();
                return result;
            });


            return Ok(res);
        }
    }
}
