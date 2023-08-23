using Final.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Final.Endpoints.WishListEnpoints.Commands
{
    [Route("api/wishlist")]
    [Authorize]
    public class AddProductToList : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<ActionResult<WishList>>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AddProductToList(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("add/{id}")]
        [SwaggerOperation(
        Summary = "Add product to WishList",
        Description = "This Api add Product to your WishList",
        OperationId = "AddProductToList",
        Tags = new[] { "WishList Endpoint" })]
        public override async Task<ActionResult<ActionResult<WishList>>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var exist = await _context.Products.FindAsync(id);
            if (exist == null) return NotFound("Product with this id does not exist");

            var user = await _userManager.GetUserAsync(User);

            bool productExists = _context.WishLists.Any(p => p.ProductId == id);
            if (productExists)
            {
                return BadRequest("This Product is already exist in your WishList");
            }

            var wishListItem = new WishList
            {
                AppUserId = user.Id,
                ProductId = id
            };

            _context.WishLists.Add(wishListItem);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
