using Final.Entity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Final.Endpoints.ProductEndpoints.Queryes
{
    [Route("api/product")]
    public class GetProductById : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<Product>
    {
        private readonly AppDbContext _context;

        public GetProductById(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "Get Product by Id",
        Description = "This Api return information about Product with a specific Id",
        OperationId = "GetProductById",
        Tags = new[] { "Product Endpoint" })]
        public override async Task<ActionResult<Product>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var res = await _context.Products.FindAsync(id);

            if (res is null) return NotFound();
            return Ok(res);
        }
    }
}
