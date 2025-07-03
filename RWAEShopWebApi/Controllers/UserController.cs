using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RWAEshopDAL.Security;
using RWAEshopDAL.Services;
using RWAEshopDAL.Models;
using Microsoft.EntityFrameworkCore;
using RWAEShopWebApi.DTOs;

namespace RWAEShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IConfiguration configuration, IUserService userService, IMapper mapper)
        {
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public ActionResult<UserRegisterDto> Register(UserRegisterDto registerDto)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if there is such a username in the database already
                var trimmedUsername = registerDto.Username.Trim();
                if (_userService.GetAllUsers().Any(x => x.Username.Equals(trimmedUsername)))
                    return BadRequest($"Username {trimmedUsername} already exists");

                // Hash the password
                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(registerDto.Password, b64salt);

                // Create user from DTO and hashed password
                var user = _mapper.Map<User>(registerDto);
                user.PwdHash = b64hash;
                user.PwdSalt = b64salt;

                // Add user and save changes to database
                _userService.CreateUser(user);

                // Update DTO Id to return it to the client
                registerDto.IdUser = user.IdUser;

                return Ok(registerDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login(UserLoginDto loginDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                // Try to get a user from database
                var existingUser = _userService.GetAllUsers()
                    .FirstOrDefault(x=> x.Username == loginDto.Username);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

                // Check is password hash matches
                var b64hash = PasswordHashProvider.GetHash(loginDto.Password, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                    return BadRequest(genericLoginFail);

                // Create and return JWT token
                var secureKey = _configuration["JWT:SecureKey"];
                int expiration = _configuration.GetValue<int>("JWT:Expiration");
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, expiration, loginDto.Username, existingUser.Role.RoleName);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
