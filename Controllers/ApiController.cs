using aspnet2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace aspnet2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataApiController : ControllerBase
    {
        private readonly MyDbContext _db;
        public ApiController(MyDbContext db) { _db = db; }

#region Gets
        [HttpGet("GetComments")]
        public async Task<IActionResult> GetComments() { return Ok(await _db.Comments.ToListAsync()); }
        [HttpGet("GetFavorites")]
        public async Task<IActionResult> GetFavorites() { return Ok(await _db.Favorites.ToListAsync()); }
        [HttpGet("GetIdeias")]
        public async Task<IActionResult> GetIdeias() { return Ok(await _db.Ideas.ToListAsync()); }
        [HttpGet("GetImages")]
        public async Task<IActionResult> GetImages() { return Ok(await _db.Images.ToListAsync()); }
        [HttpGet("GetPosts")]
        public async Task<IActionResult> GetPosts() { return Ok(await _db.Posts.ToListAsync()); }
        [HttpGet("GetUpvote")]
        public async Task<IActionResult> GetUpvote() { return Ok(await _db.Upvotes.ToListAsync()); }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers() { return Ok(await _db.Users.ToListAsync()); }
#endregion
#region Posts        
        [HttpPost("PostComments")]
        public async Task<IActionResult> PostComments(Comment coment) 
        {
            await _db.Comments.AddAsync(coment);
            await _db.SaveChangesAsync();
            return Ok(coment);
        }
        [HttpPost("PostFavorites")]
        public async Task<IActionResult> PostFavorites(Favorite favorite)
        {
            await _db.Favorites.AddAsync(favorite);
            await _db.SaveChangesAsync();
            return Ok(favorite);
        }
        [HttpPost("PostIdeias")]
        public async Task<IActionResult> PostIdeias(Idea idea)
        {
            await _db.Ideas.AddAsync(idea);
            await _db.SaveChangesAsync();
            return Ok(idea);
        }
        [HttpPost("PostImages")]
        public async Task<IActionResult> PostImages(Image image)
        {
            await _db.Images.AddAsync(image);
            await _db.SaveChangesAsync();
            return Ok(image);
        }
        [HttpPost("PostPosts")]
        public async Task<IActionResult> PostPosts(Post post)
        {
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
            return Ok(post);
        }
        [HttpPost("PostUpvote")]
        public async Task<IActionResult> PostUpvote(Upvote upvote)
        {
            await _db.Upvotes.AddAsync(upvote);
            await _db.SaveChangesAsync();
            return Ok(upvote);
        }
        [HttpPost("PostUsers")]
        public async Task<IActionResult> PostUsers(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return Ok(user);
        }
#endregion
    }
}
