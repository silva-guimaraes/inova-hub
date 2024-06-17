using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspnet2.Models;
using Microsoft.EntityFrameworkCore;
// using System.Linq;
namespace aspnet2.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{

    private int usuarioLogado {get; set;}
    private readonly MyDbContext db;
    public HomeController(MyDbContext _db)
    {
        db = _db;
    }

    string noImage = "360_F_473254957_bxG9yf4ly7OBO5I0O5KABlN930GwaMQz.jpg";

    [Route("")]
    public IActionResult Index() { return View();  }

    [Route("Feed/{ultimo?}")]
    public async Task<IActionResult> Feed(int? ultimo)
    {
        System.Console.WriteLine(ultimo);
        if (ultimo == null) { ultimo = int.MinValue; }
        var usuarioPadrao = getUsuarioPadrao();
        var query = await db.Ideas.Include(x => x.Upvotes)
            .Include(x => x.Images)
            .Where(x => x.Id > ultimo)
            .OrderBy(x => x.Id)
            .Take(3).ToListAsync();

        var model = query.Select(
                (idea, index) => new IdeaViewModel { 
                        Idea = idea,
                        UserUpvoted = idea.Upvotes.Any(x => x.UserId == usuarioPadrao!.Id),
                        imgUrl = idea.Images.Count > 0 ? idea.Images.ToList()[0].Url : noImage,
                    })
            .ToList();
        return View("FeedIdea", model);
    }

    [Route("Ideia/{id?}")]
    public async Task<IActionResult> Idea(int? id)
    {
        var usuarioPadrao = getUsuarioPadrao();

        var query = await db.Ideas.Include(x => x.Upvotes)
            .Include(x => x.Images)
            .Where(x => x.Id == id)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

        var model = new IdeaViewModel { Idea = query,
          UserUpvoted = query.Upvotes.Any(x => x.UserId == usuarioPadrao!.Id),
          imgUrl = query.Images.Count > 0 ? query.Images.ToList()[0].Url : noImage,
        };

        return View(model);
    }

    [Route("Usuario/{id?}")]
    public IActionResult Usuario(int? id)
    {
        var query = db.Users
            .Include(x => x.Ideas)
            .ThenInclude(x => x.Images)
            .FirstOrDefault(x => x.Id == id);
        if (query == null) {
            return NotFound();
        }
        var model = query;
        var user = db.Users.Find(id);
        ViewBag.Nome = user.Name;
        ViewBag.Email = user.Email;
        usuarioLogado = user.Id;
        TempData["usuarioemsessao"] = usuarioLogado;
        Console.WriteLine(usuarioLogado);
        return View("Perfil", model);
    }

    // Temporário?
    public User? getUsuarioPadrao() { return db.Users.FirstOrDefault(x => x.Id == 3); }

    [Route("Downvote/{id?}")]
    public void Downvote(int id)
    {
        var query = db.Ideas.FirstOrDefault(x => x.Id == id);
        var usuarioPadrao = getUsuarioPadrao();
        if (query == null || usuarioPadrao == null) {
            Response.StatusCode = 404;
            return;
        }
        var upvote = db.Upvotes.Where(x => x.IdIdea == id && x.UserId == usuarioPadrao.Id).ExecuteDelete();
        db.SaveChanges();
    }

    [Route("Upvote/{id?}")]
    public void Upvote(int id)
    {
        var query = db.Ideas.FirstOrDefault(x => x.Id == id);
        var usuarioPadrao = getUsuarioPadrao();
        if (query == null || usuarioPadrao == null) {
            Response.StatusCode = 404;
            return;
        }
        try
        {
            db.Add(new Upvote {
                        User = usuarioPadrao,
                        IdIdea = id,
                        UserId = usuarioPadrao.Id,
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
        Response.StatusCode = 204;
    }

    // oque diabos é isso? já existe uma rota cadastrar...
    [Route("Cadastro")]
    public IActionResult Cadastro() { return View(); }

    // fixme: aportuguesar
    [Route("CreateIdea")]
    public IActionResult CreateIdea() { return View(); }

    [HttpPost]
    [Route("CreateIdea")]
    public async Task<Object> CriarIdeia(string nome, string desc, string categoria, List<IFormFile> imagens,
            string conteudo) 
    {

        var usuarioPadrao = getUsuarioPadrao();

        System.Console.WriteLine("nova ideia!");
        System.Console.WriteLine(nome);
        System.Console.WriteLine(desc);
        System.Console.WriteLine(categoria);
        System.Console.WriteLine(conteudo);

        var newIdea = new Idea { Text = desc, Title = nome, IdUser = usuarioPadrao.Id, Content = conteudo};
        // db.Ideas.Add(newIdea);

        long size = imagens.Sum(f => f.Length);

        foreach (var formFile in imagens)
        {
            if (formFile.Length > 0)
            {
                String[] path = { Environment.CurrentDirectory,"wwwroot", "images", formFile.FileName };

                
                var filePath = Path.Combine(path);
                System.Console.WriteLine(filePath);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await formFile.CopyToAsync(stream);
                }

                db.Images.Add(new Image { Url = formFile.FileName, Idea = newIdea });
            }
        }

        db.SaveChanges();

        return Ok(new { id = newIdea.Id, count = imagens.Count, size });
    }
    

    [Route("Perfil")]
    public IActionResult Perfil() { return View(); }

    [Route("Contacts")]
    public IActionResult Contacts() { return View(); }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    [HttpPost]
    [Route("Cadastrar")]
    public IActionResult Cadastrar(string nome, string email, string senha)
    {
        var user = new User {
            Name = nome,
            Email = email,
            Password =  senha
        };
        db.Users.Add(user);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("VerificarLogin")]
    public IActionResult VerificarLogin(string login, string pass)
    {
        var user = db.Users.FirstOrDefault(x => x.Email == login && x.Password == pass);
        if (user == null) { return NotFound("Usuario Não Encontrado!"); }
        return Usuario(user.Id);
    }

    [HttpPost]
    [Route("EditarUsuario")]
    public IActionResult EditarUsuario(string nome, string email, string senha)
    {
      Console.WriteLine(usuarioLogado);
      var usuario = db.Users.Find(TempData["usuarioemsessao"]);
      if (usuario != null)
      {
         usuario.Name = nome;
         usuario.Email = email;
         usuario.Password = senha;
         db.SaveChanges();
         return Usuario((int)TempData["usuarioemsessao"]);
      } else { return NotFound(":( Erro, contate ao ademiro!"); }



   }

    [HttpDelete]
    [Route("Delete/{id?}")]
    public IActionResult Delete(int id)
    {
        var ideia = db.Ideas.Find(id);
        if (ideia != null)
        {
            db.Ideas.Remove(ideia);
            return Usuario((int)TempData["usuarioemsessao"]);
        }
        else { return NotFound("Erro desconhecido.");}
    }
}
