using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Context;
using Tryitter.DTOs;
using Tryitter.Models;
using Tryitter.Services;

namespace Tryitter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/register")]
        public ActionResult Post(UserDTO userDTO)
        {
            var emailExist = _context.Users!.FirstOrDefault(user => user.Email == userDTO.Email);
            if (emailExist != null)
                return BadRequest("Email já esta cadastrado");

            User user = new()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Admin = userDTO.Admin,
            };

            _context.Users!.Add(user);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetUser",
                new { id = user.UserId }, userDTO);
        }

        [HttpPost]
        [Route("/Login")]
        [AllowAnonymous]
        public ActionResult Login(AuthDTO authRequest)
        {
            var existingUser = _context.Users!.FirstOrDefault(user => user.Email == authRequest.Email && user.Password == authRequest.Password);
            if (existingUser is null)
                return Unauthorized("Usuário inválido.");

            var jwtAuthenticationManager = new TokenGenerator();
            var authResult = jwtAuthenticationManager.Generate(authRequest);

            return Ok(authResult);
        }
    }
}
