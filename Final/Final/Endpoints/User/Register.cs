using Final.Endpoints.ProductEndpoints.Models;
using Final.Endpoints.User.Models;
using Final.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace Final.Endpoints.User
{
    [Route("api/user")]
    public class Register : EndpointBaseAsync
        .WithRequest<LoginModel>
        .WithActionResult<LoginModel>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public Register(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("register")]
        [SwaggerOperation(
        Summary = "Create a new account",
        Description = "If you don't have any account - you can create it",
        OperationId = "CreateNewAccount",
        Tags = new[] { "User Endpoint" })]
        public override async Task<ActionResult<LoginModel>> HandleAsync([FromBody] LoginModel model, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again. Maybe the password is not strong" });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}
