using System.Threading.Tasks;
using MagicVillaAPI.DTO;
using MagicVillaAPI.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagicVillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly JWTService jwtService;
        public AuthController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, JWTService _jwtService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            jwtService = _jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserDTO user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Username
            };

            var result = await userManager.CreateAsync(identityUser, user.Password);
            System.Console.WriteLine(result);
            if (!result.Succeeded)
            {
                return BadRequest("Error in User registeration: Creation");
            }

            result = await userManager.AddToRoleAsync(identityUser, user.Role);
            if (!result.Succeeded)
            {
                return BadRequest("Error in User registeration: Adding roles");
            }

            return Ok("User registered.");
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDTO user)
        {
            var identityUser = await userManager.FindByNameAsync(user.Username);
            if (identityUser == null)
            {
                return BadRequest("No user exists.");
            }

            var result = await signInManager.CheckPasswordSignInAsync(identityUser, user.Password, false);
            if (!result.Succeeded)
            {
                return BadRequest("Incorrect password.");
            }

            var roles = await userManager.GetRolesAsync(identityUser);

            var token = jwtService.GenerateToken(identityUser, roles);

            return Ok(new { token });
        }
    }
}
