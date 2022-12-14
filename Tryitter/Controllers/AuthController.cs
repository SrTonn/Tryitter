using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Context;
using Tryitter.DTOs;
using Tryitter.Models;
using Tryitter.Services;
using Tryitter.Validations;

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
            if(!UserValidation.IsValidEmail(userDTO.Email!))
                return BadRequest("Formato inválido");

            var emailExist = _context.Users!.FirstOrDefault(user => user.Email == userDTO.Email);
            if (emailExist != null)
                return BadRequest("Email já cadastrado");

            if (userDTO.Admin)
            {
                var hasToken = Request.Headers.TryGetValue("Authorization", out var bearerToken);
                if(!hasToken)
                    return Unauthorized("Token não encontrado");

                if (!UserValidation.IsAdminUser(bearerToken))
                    return Unauthorized("Acesso negado");
            }

            User user = new()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Admin = userDTO.Admin,
            };

            _context.Users!.Add(user);
            _context.SaveChanges();

            var result = new CreatedAtRouteResult("GetUser",
                new { id = user.UserId }, userDTO);

            userDTO.id = user.UserId;

            return result;
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
            var authResult = jwtAuthenticationManager.Generate(existingUser.Email, existingUser.Admin);

            return Ok(authResult);
        }
    }
}
