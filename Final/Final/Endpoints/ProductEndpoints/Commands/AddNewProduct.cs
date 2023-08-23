using Final.Endpoints.ProductEndpoints.Models;
using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.ProductEndpoints.Commands
{
    [Route("api/product")]
    [Authorize(Roles = "Manager")]
    public class AddNewProduct : EndpointBaseAsync
        .WithRequest<ProductModel>
        .WithActionResult<ProductModel>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;

        public AddNewProduct(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPost("add")]
        [SwaggerOperation(
        Summary = "Add new Product",
        Description = "This Api add information about new Product in DB",
        OperationId = "AddNewProduct",
        Tags = new[] { "Product Endpoint" })]
        public override async Task<ActionResult<ProductModel>> HandleAsync([FromBody] ProductModel productModel, CancellationToken cancellationToken)
        {
            bool productExists = _context.Products.Any(p => p.ProductName.Equals(productModel.ProductName));
            if (productExists)
            {
                return BadRequest("This Product is already exist");
            }

            var product = new Product
            {
                ProductName = productModel.ProductName,
                ImagePath = productModel.ImagePath,
                price = productModel.price// Обратите внимание, что я изменил "price" на "Price"
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            _cache.Remove("product_data");
            return Ok();

        }
    }
}
