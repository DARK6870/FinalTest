using Final.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Final.Endpoints.WishListEnpoints.Commands
{
    [Route("api/wishlist")]
    [Authorize]
    public class DeleteProductFromList : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<ActionResult<WishList>>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteProductFromList(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpDelete("delete/{id}")]
        [SwaggerOperation(
        Summary = "Delete product from WishList",
        Description = "This Api delete Product from your WishList",
        OperationId = "DeleteProductFromList",
        Tags = new[] { "WishList Endpoint" })]
        public override async Task<ActionResult<ActionResult<WishList>>> HandleAsync(int id, CancellationToken cancellationToken)
        {

            var user = await _userManager.GetUserAsync(User);
            var exist = await _context.WishLists.SingleOrDefaultAsync(x => x.AppUserId == user.Id && x.ProductId == id, cancellationToken);

            if (exist == null)
                return NotFound();

            _context.WishLists.Remove(exist);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
