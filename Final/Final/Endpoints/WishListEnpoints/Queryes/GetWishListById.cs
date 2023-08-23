using Final.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace Final.Endpoints.WishListEnpoints.Queryes
{
    [Route("api/wishlist")]
    [Authorize(Roles = "Manager")]
    public class GetWishListById : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<WishList>
    {
        private readonly AppDbContext _context;

        public GetWishListById(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "Get WishList by Id",
        Description = "This Api return WishList with a specific Id",
        OperationId = "GetWishListById",
        Tags = new[] { "WishList Endpoint" })]
        public override async Task<ActionResult<WishList>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var res = await _context.WishLists.FindAsync(id);
            if (res == null) return NotFound();

            res.Product = await _context.Products.FindAsync(res.ProductId);

            var result = new
            {
                res.AppUserId,
                res.Product,
            };

            return Ok(result);
        }
    }
}
