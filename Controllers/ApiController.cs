using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspnet2.Controllers;


    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : Controller
    {

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            return Ok("pong");
        }
    }
