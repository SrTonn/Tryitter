using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tryitter.Context;
using Tryitter.Models;
using Tryitter.Validations;

namespace Tryitter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll()
        {
            var posts = _context.Posts!.AsNoTracking();
            if (posts is null)
                return NotFound("Posts não encontrados...");

            return Ok(posts);
        }

        [HttpGet("{id:int}", Name = "GetPost")]
        public ActionResult<IEnumerable<Post>> GetById(int id)
        {
            var post = _context.Posts!.FirstOrDefault(post => post.PostId == id);
            if(post is null)
                return NotFound("Post não encontrado.");

            return Ok(post);
        }

        [HttpPost]
        public ActionResult Post(Post post)
        {
            if (post is null)
                return BadRequest();

            if (!UserValidation.IsValid(_context.GetUser(post.UserId)))
                return NotFound("Usuário não encontrado.");

            _context.Posts!.Add(post);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetPost",
                new { id = post.PostId }, post);
        }

        [HttpPut]
        public ActionResult Put(int id, Post post)
        {
            if (id != post.PostId)
                return BadRequest();

            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(post);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var post = _context.Posts!.FirstOrDefault(post => post.PostId == id);

            if (post is null)
                return NotFound("Usuário não localizado.");

            _context.Posts!.Remove(post);
            _context.SaveChanges();

            return Ok(post);
        }
    }
}
