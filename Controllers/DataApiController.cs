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
        public DataApiController(MyDbContext db) { _db = db; }

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
    }
}
