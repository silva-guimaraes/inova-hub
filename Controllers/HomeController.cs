using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspnet2.Models;
// using System.Web.Helpers;

namespace aspnet2.Controllers;

public class HomeController : Controller
{
    private readonly MyDbContext db;

    public HomeController(MyDbContext _db)
    {
        db = _db;
    }

    [Route("")]
    public IActionResult Index() {
        return View(db.Ideas.ToList());
    }

    [Route("teste")]
    public IActionResult Teste() {
        return View();
    }

    // [Route("json")]
    // public String Json() {
    //     return System.Web.
    // }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
