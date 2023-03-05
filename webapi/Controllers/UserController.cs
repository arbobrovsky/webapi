using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using webapi.Data.Models;
using webapi.IRepository;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private IUserRepository _oUserRepository;

        public UserController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _oUserRepository = userRepository;
        }

        [HttpGet]
        [Route("get")]
        public string test()
        {
            return "WORK";
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("GetUser/{id}")]
        public async Task<User> GetUser(Guid id)
        {
            return await _oUserRepository.Get(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("SaveUser")]
        public async Task<IActionResult> Save(User user)
        {
            if (user.UserId != Guid.Empty)
            {
                return Ok(await _oUserRepository.Save(user));
            }
            else return StatusCode((int)HttpStatusCode.InternalServerError, "Some ERROR!");
        }

        [HttpPost]
        [Route("SaveUserFromRegistration")]
        public async Task<IActionResult> SaveFromRegionstration(User user)
        {
            if (user.UserId == Guid.Empty)
            {
                return Ok(await _oUserRepository.Save(user));
            }
            else return StatusCode((int)HttpStatusCode.InternalServerError, "Some ERROR!");
        }



        [HttpGet]
        [Route("Signin/{username}/{password}")]
        public async Task<IActionResult> Signin(string username, string password)
        {
            try
            {
                User model = new User()
                {
                    Username = username,
                    Password = password
                };
                var user = await _oUserRepository.GetByUsernamePassword(model);
                if (user.UserId == Guid.Empty) return StatusCode((int)HttpStatusCode.NotFound, "Имя пользователя или пароль введен неверно!");
                user.Token = GenerateToken(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private string GenerateToken(User model)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Username) };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
