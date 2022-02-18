using Microsoft.AspNetCore.Mvc;

namespace webapp.Controllers;

public class InvoicesController : Controller
{
    public IActionResult New() => View();
}