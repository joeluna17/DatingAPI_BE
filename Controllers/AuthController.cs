using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data_;
using DatingApp.API.DTO;
using DatingApp.API.Modles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase {
        public IAuthRepository _repo;

        public IConfiguration _config; 

        public AuthController (IAuthRepository repo, IConfiguration config) {
            _repo = repo;
            _config = config;
        }

    [HttpPost ("register")]
    public async Task<IActionResult> Register(UserForRegister userForRegister){
        
        //validate request
        userForRegister.Username = userForRegister.Username.ToLower();
            if(await _repo.UserExists(userForRegister.Username))
                return  BadRequest("Username already exists");

        var userToCreate = new User {
            Username =userForRegister.Username
        };

        var createdUser = await _repo.Register(userToCreate, userForRegister.Password);

        return StatusCode(201);

     }

     [HttpPost ("login")]
     public async Task<IActionResult> Login(UserForLogin userForLogin){
         var userFromRepo = await _repo.Login(userForLogin.Username.ToLower(), userForLogin.Password);
         if(userFromRepo == null)
            return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username )
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
     }

    }
}