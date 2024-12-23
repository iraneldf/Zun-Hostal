using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers;

[Route("/")]
public class HomeController : Controller
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Index()
    {
        try
        {
            var html = await System.IO.File.ReadAllTextAsync("wwwroot/index.html");
            return base.Content(html, "text/html");
        }
        catch (Exception)
        {
            //_logger.ErrorFormat("Unhandled exception get html admin: {0}. {1}", e.Message, e.StackTrace);
            return base.Content("<h6 style=\"color:red\">An unexpected error occurred</h6>", "text/html");
            ;
        }
    }
}