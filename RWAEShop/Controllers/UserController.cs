using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RWAEshopDAL.Security;

namespace RWAEShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public ActionResult GetToken()
        {


            try
            {

                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
