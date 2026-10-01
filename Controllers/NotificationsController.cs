using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EurasiaBus.Controllers;

[Authorize]
public class NotificationsController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}