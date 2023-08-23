using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.TypesEndpoints.Queryes
{
    [Route("api/types")]
    public class GetAllTypes : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<Product>>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public GetAllTypes(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        [SwaggerOperation(
        Summary = "Get all Types",
        Description = "This Api return full list of Product Types",
        OperationId = "GetAllTypes",
        Tags = new[] { "Types Endpoint" })]
        public override async Task<ActionResult<IEnumerable<Product>>> HandleAsync(CancellationToken cancellationToken)
        {
            var res = await _cache.GetOrAddAsync("types_data", async () =>
            {
                var result = await _context.Types.ToListAsync();
                return result;
            });

            return Ok(res);
        }
    }
}
