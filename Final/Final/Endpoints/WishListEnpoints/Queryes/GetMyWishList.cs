using Final.Entity;
using LazyCache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.WishListEnpoints.Queryes
{   
    
    public class GetMyWishList : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<WishList>>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public GetMyWishList(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("api/wishlist/my")]
        [SwaggerOperation(
        Summary = "Get my WishList",
        Description = "This Api return your full Wish List",
        OperationId = "GetMyWishList",
        Tags = new[] { "WishList Endpoint" })]
        public override async Task<ActionResult<IEnumerable<WishList>>> HandleAsync(CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);

            var res = await _context.WishLists.Where(c => c.AppUserId == user.Id).ToListAsync();
            var productCounts = res
                .GroupBy(wl => wl.ProductId)
                .ToDictionary(group => group.Key, group => group.Count());

            foreach (var wishList in res)
            {
                wishList.Product = await _context.Products.FindAsync(wishList.ProductId);
            }

            var result = res.Select(wishList => new
            {
                wishList.Product,
                OtherWishListCount = productCounts.ContainsKey(wishList.ProductId) ? productCounts[wishList.ProductId] : 0
            });

            return Ok(result);
        }
    }
}
