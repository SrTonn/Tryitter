using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tryitter.Context;
using Tryitter.DTOs;
using Tryitter.Models;
using Tryitter.Validations;

namespace Tryitter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<UserDTO>> GetAll()
        {
            var users = _context.Users!.AsNoTracking();
            if (users is null)
                return NotFound("Usuários não encontrados...");

            return Ok(users.Select(u => new
            {
                Id = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Admin = u.Admin
            }));
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public ActionResult<User> GetById(int id)
        {
            var user = _context.GetUser(id);
            if (UserValidation.IsValidUser(user))
                return Ok(user);

            return NotFound("Usuário não encontrado.");
        }

        [HttpPut("{id:int}", Name = "Put Admin")]
        [Authorize(Policy = "TryitterAdministrators")]
        public ActionResult UpdateByAdmin(int id, UserDTO input)
        {
            var user = _context.GetUser(id);
            if (!UserValidation.IsValidUser(user))
                return BadRequest("Usuário não encontrado.");

            user.UserId = id;
            user.Name = input.Name;
            user.Email = input.Email;
            user.Password = input.Password;
            user.Admin = input.Admin;

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPut("me")]
        public ActionResult UpdateByUser(OwnUser ownUser)
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var email = claims.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
            var user = _context.GetUser(email);

            user.Email = ownUser.Email;
            user.Name = ownUser.Name;
            user.Password = ownUser.Password;

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id:int}", Name = "Delete Admin")]
        [Authorize(Policy = "TryitterAdministrators")]
        public ActionResult DeleteByAdmin(int id)
        {
            var user = _context.Users!.FirstOrDefault(user => user.UserId == id);
            if (user is null)
                return NotFound("Usuário não localizado.");

            _context.Users!.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("me")]
        public ActionResult DeleteByUser()
        {
            var claims = HttpContext.User.Identity as ClaimsIdentity;
            var email = claims.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
            var user = _context.GetUser(email);

            if (user is null)
                return NotFound("Usuário não localizado.");

            _context.Users!.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
