using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Entity;
using webapp.ViewModel;

namespace webapp;

[Authorize]
public class InvoiceController : Controller
{
    private readonly AppDbContext _ctx;
    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(ILogger<InvoiceController> logger, AppDbContext context)
    {
        _logger = logger;
        _ctx = context;
    }

    [HttpGet]
    public IActionResult Index() => 
  
    
    // [HttpGet]
    // public IActionResult Create() => View();

    // [HttpPost]
    // public async Task<IActionResult> Create(NewInvoiceViewModel model)
    // {
    //     if(!ModelState.IsValid)
    //     {
    //         return View();
    //     }

    //     var inv = new Invoice()
    //     {
            
    //     };

    //     await _ctx.Invoice.AddAsync(inv);

    //     await _ctx.SaveChangesAsync();

    //     return RedirectToAction(nameof(Created));
    // }

    
}