using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspnet2.Models;
using aspnet2.Services;
using Microsoft.EntityFrameworkCore;

namespace aspnet2.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{
    private readonly MyDbContext db;

    public HomeController(MyDbContext _db)
    {
        db = _db;
    }

    [Route("")]
    public IActionResult Index() {
        return View();
    }

    // retorna ideias que serão servidas no feed
    [Route("Serve/{last?}")]
    public async Task<IActionResult> Feed(int? last) {

        // ultima ideia que o cliente tem. queremos procurar ideias que venham depois dessa obviamente
        if (last == null)
            last = int.MinValue;

        var defaultUser = getDefaultUser();

        var query = await db.Ideas.Include(x => x.Upvotes)
            .Where(x => x.Id > last)
            .OrderBy(x => x.Id)
            .Take(3).ToListAsync();

        // passa as ideias pra view que retorna html puro pro cliente
        ViewBag.posts = query.Select((idea, index) => new { 
                        Idea = idea,
                        // true se usuário já tiver dado upvote nessa ideia
                        UserUpvoted = idea.Upvotes.Any(x => x.UserId == defaultUser.Id)
                    });

        return View("FeedIdea");
    }

    [Route("Idea/{id?}")]
    public IActionResult Idea(int? id) {
        var query = db.Ideas.Where(x => x.Id == id).Single();

        ViewBag.Idea = query;
        ViewBag.Title = query.Title;
       
        return View();
    }

    // usuário padrão para que nós possamos testar as funcionalidades que requerem um login.
    // isso é temporário como o combinado.
    public User? getDefaultUser() {
        return db.Users.FirstOrDefault(x => x.Id == 3);
    }

    // não um downvote em si. isso apenas remove o upvote.
    [Route("Downvote/{id?}")]
    public void Downvote(int id) {

        var query = db.Ideas.FirstOrDefault(x => x.Id == id);

        var defaultUser = getDefaultUser();


        if (query == null || defaultUser == null) {
            Response.StatusCode = 404;
            return;
        }

        var upvote = db.Upvotes.Where(x => x.IdIdea == id && x.UserId == defaultUser.Id).ExecuteDelete();

        db.SaveChanges();
    }

    [Route("Upvote/{id?}")]
    public void Upvote(int id) {

        var query = db.Ideas.FirstOrDefault(x => x.Id == id);

        var defaultUser = getDefaultUser();

        if (query == null || defaultUser == null) {
            Response.StatusCode = 404;
            return;
        }

        try
        {
            db.Add(new Upvote {
                        User = defaultUser,
                        IdIdea = id,
                        UserId = defaultUser.Id,
                        // fixme: datetime, e não dateonly
                        UpvoteDate = DateOnly.FromDateTime(DateTime.Now),
                        IdIdeaNavigation = query,
                    });

            db.SaveChanges();
        }
        catch (DbUpdateException)
        {
            System.Console.WriteLine("duplicate");
            Response.StatusCode = 401;
            return;
        }
    }

    [Route("Cadastro")]
    public IActionResult Cadastro() {
        return View();
    }

    [Route("Contacts")]
    public IActionResult Contacts()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // https://balta.io/blog/aspnet-5-autenticacao-autorizacao-bearer-jwt
    [HttpPost]
    [Route("VerificarLogin")]
    public ActionResult<dynamic> VerificarLogin(string login, string pass)
    {
        var user = db.Users.FirstOrDefault(x => x.Email == login && x.Password == pass);

        if (user == null) 
            return NotFound(new { message = "Usuário ou senha inválidos" });

        // Gera o Token
        var token = TokenService.GenerateToken(user);

        // Passa token em header de resposta para o navegador
        Response.Headers.Add("Authorization", "Bearer " + token);

        // Retorna os dados
        return new { token = token };
    }
}
