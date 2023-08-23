using Final.Endpoints.ProductEndpoints.Models;
using Final.Endpoints.ProductTypeEndpoints.Models;
using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.ProductTypeEndpoints.Commands
{
    [Route("api/productTypes")]
    [Authorize(Roles = "Manager")]
    public class AddNewProductType : EndpointBaseAsync
        .WithRequest<PTModel>
        .WithActionResult<PTModel>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public AddNewProductType(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPost("add")]
        [SwaggerOperation(
        Summary = "Add new ProductType",
        Description = "This Api add information about new ProductType in DB",
        OperationId = "AddNewProductType",
        Tags = new[] { "ProductTypes Endpoint" })]
        public override async Task<ActionResult<PTModel>> HandleAsync([FromBody] PTModel ptModel, CancellationToken cancellationToken)
        {
            bool ptexist = _context.ProductTypes.Any(p => p.TypeId.Equals(ptModel.TypeId) && p.ProductId.Equals(ptModel.ProductId));
            if (ptexist)
            {
                return BadRequest("This ProductType is already exist");
            }

            var pt = new ProductTypes
            {
                ProductId = ptModel.ProductId,
                TypeId = ptModel.TypeId
            };

            await _context.ProductTypes.AddAsync(pt);
            await _context.SaveChangesAsync();

            _cache.Remove("producttype_data");

            return Ok();
        }
    }
}
