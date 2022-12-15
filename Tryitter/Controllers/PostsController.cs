using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tryitter.Context;
using Tryitter.Models;
using Tryitter.Validations;

namespace Tryitter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Post>> GetAll()
        {
            var posts = _context.Posts!.AsNoTracking();
            if (posts is null)
                return NotFound("Posts não encontrados...");

            return Ok(posts);
        }

        [HttpGet("{id:int}", Name = "GetPost")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Post>> GetById(int id)
        {
            var post = _context.Posts!.AsNoTracking().FirstOrDefault(post => post.PostId == id);
            if (post is null)
                return NotFound("Post não encontrado.");

            return Ok(post);
        }       
        
        [HttpGet("last",Name = "GetLastPost")]
        [AllowAnonymous]
        public ActionResult<Post> GetLastPost()
        {
            var post = _context.Posts!.OrderBy(x => x.PostId).LastOrDefault();
            if (post is null)
                return NotFound("Post não encontrado.");

            return Ok(post);
        }

        [HttpGet("last/{id:int}", Name = "GetLastPostByUserId")]
        [AllowAnonymous]
        public ActionResult<Post> GetLastPostByUserId(int id)
        {
            var post = _context.Posts!
                .Where(x => x.UserId == id)
                .OrderBy(x => x.PostId)
                .LastOrDefault();

            if (post is null)
                return NotFound("Post não encontrado.");

            return Ok(post);
        }

        [HttpPost]
        public ActionResult Post(Post post)
        {
            if (post is null)
                return BadRequest();

            Request.Headers.TryGetValue("Authorization", out var bearerToken);

            var user = _context.GetUser(bearerToken);

            if (!UserValidation.IsValidUser(user))
                return NotFound("Usuário não encontrado ao tentar relacionar o newPost.");

            post.UserId = user!.UserId;

            _context.Posts!.Add(post);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetPost",
                new { id = post.PostId }, post);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Post newPost)
        {
            Request.Headers.TryGetValue("Authorization", out var bearerToken);

            var user = _context.GetUser(bearerToken)!;
            var post = _context.Posts!.FirstOrDefault(p => p.PostId == id)!;

            if (id != post.PostId)
                return BadRequest("Post não encontrado");

            if (post.UserId != user.UserId)
                return BadRequest("Não autorizado");

            post.Title = newPost.Title;
            post.Description = newPost.Description;
            post.ImageUrl = newPost.ImageUrl;

            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(newPost);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var post = _context.Posts!.FirstOrDefault(post => post.PostId == id);

            if (post is null)
                return NotFound("Post não localizado.");

            _context.Posts!.Remove(post);
            _context.SaveChanges();

            return Ok();
        }
    }
}
