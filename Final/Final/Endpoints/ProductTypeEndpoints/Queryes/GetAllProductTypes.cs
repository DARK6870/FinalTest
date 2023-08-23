using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.ProductTypeEndpoints.Queryes
{
    [Route("api/productTypes")]
    public class GetAllProductTypes : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<ProductTypes>>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public GetAllProductTypes(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        [SwaggerOperation(
        Summary = "Get all ProductTypes",
        Description = "This Api return full list of ProductTypes",
        OperationId = "GetAllProductTypes",
        Tags = new[] { "ProductTypes Endpoint" })]
        public override async Task<ActionResult<IEnumerable<ProductTypes>>> HandleAsync(CancellationToken cancellationToken)
        {
            var res = await _cache.GetOrAddAsync("producttype_data", async () =>
            {
                var result = await _context.ProductTypes.ToListAsync();
                return result;
            });

            return Ok(res.Select(c => new
            {
                c.Id,
                c.ProductId,
                c.TypeId
            }));
        }
    }
}
