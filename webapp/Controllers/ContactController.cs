
using System;
using Microsoft.AspNetCore.Mvc;
using webapp.Data;
using webapp.Entity;

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
       var contact =_dbcontext.Contacts.FirstOrDefault(w=>w.Id==Id);
        if(contact==null){
            return NotFound();
        }
        return View(contact);
      
    }
    [HttpGet]
    public IActionResult GetContacts()
    {
        var contacts= _dbcontext.Contacts.ToList();
        if(contacts==null){
            return NotFound();
        }
        return View(contacts);
    }

    [HttpPost]
    public IActionResult Contact([FromForm] Contact model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        _dbcontext.Contacts.Add(model);
        _dbcontext.SaveChanges();
        return View();
    }
    [HttpPut]
    public IActionResult Contact(Guid ID)
    {
        try
        {
          var contact=_dbcontext.Contacts.FirstOrDefault(p=>p.Id==ID); 
          _dbcontext.Contacts.Update(contact);
          _dbcontext.SaveChanges();
          return View(contact);
        }
        catch(Exception e)
        {
          return BadRequest(e.Message);
        }       
    }

    [HttpGet]
    public IActionResult DeleteContact(Guid Id)
    {
        try
        {
          var contact=_dbcontext.Contacts.FirstOrDefault(p=>p.Id==Id); 
          _dbcontext.Contacts.Remove(contact);
          _dbcontext.SaveChanges();
          return View();
        }
        catch(Exception e)
        {
          return BadRequest(e.Message);
        }      
    }
}