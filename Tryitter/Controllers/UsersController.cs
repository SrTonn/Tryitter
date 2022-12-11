using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public ActionResult<User> Get(int id)
        {
            var user = _context.GetUser(id);
            if (UserValidation.IsValid(user))
                return Ok(user);

            return NotFound("Usuário não encontrado.");
        }

        [HttpPost]
        public ActionResult Post(User user)
        {
            if (user is null)
                return BadRequest();

            _context.Users!.Add(user);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetUser",
                new { id = user.UserId }, user);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, User user)
        {
            if (!UserValidation.IsValid(_context.GetUser(id)))
                return BadRequest("Usuário não encontrado.");

            user.UserId = id;

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var user = _context.Users!.FirstOrDefault(user => user.UserId == id);

            if (user is null)
                return NotFound("Usuário não localizado.");

            _context.Users!.Remove(user);
            _context.SaveChanges();

            return Ok(user);
        }
    }
}
