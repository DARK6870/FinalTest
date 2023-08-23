using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Final.Endpoints.WishListEnpoints.Queryes
{
    [Route("api/wishlist")]
    [Authorize(Roles = "Manager")]
    public class GetAllWishList : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<WishList>>
    {
        private readonly AppDbContext _context;

        public GetAllWishList(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        [SwaggerOperation(
        Summary = "Get all WishList",
        Description = "This Api return full list of Wish List",
        OperationId = "GetALlWishLists",
        Tags = new[] { "WishList Endpoint" })]
        public override async Task<ActionResult<IEnumerable<WishList>>> HandleAsync(CancellationToken cancellationToken)
        {
            var res = await _context.WishLists.ToListAsync();
            var productCounts = res
                .GroupBy(wl => wl.ProductId)
                .ToDictionary(group => group.Key, group => group.Count());

            foreach (var wishList in res)
            {
                wishList.Product = await _context.Products.FindAsync(wishList.ProductId);
            }

            var result = res.Select(wishList => new
            {
                wishList.AppUserId,
                wishList.Product,
                OtherWishListCount = productCounts.ContainsKey(wishList.ProductId) ? productCounts[wishList.ProductId] : 0
            });

            return Ok(result);
        }
    }
}
