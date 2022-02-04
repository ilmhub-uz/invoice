using invoice.Data;
using Microsoft.AspNetCore.Mvc;

namespace webapp.Controllers;


public class ContactController : Controller
{
    private readonly ILogger<ContactController> _logger;
    private readonly AppDbContext _dbcontext;

    public ContactController(ILogger<ContactController> logger, AppDbContext dbcontext)
    {
        _logger = logger;
        _dbcontext = dbcontext;
    }

    
}