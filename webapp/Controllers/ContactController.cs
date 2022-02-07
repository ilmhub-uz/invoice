
using System;
using Microsoft.AspNetCore.Mvc;
using webapp.Data;

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

    [HttpGet]
    public IActionResult GetContact(Guid Id)
    {
       
       
       return View();
    }
    [HttpGet]
    public IActionResult GetContacts()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Contact()
    {
        return View();
    }
    [HttpPut]
    public IActionResult Contact(Guid ID)
    {
        return View();
    }

    [HttpGet]
    public IActionResult DeleteContact(Guid Id)
    {
        return View();
    }
}