using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Entity;
using webapp.ViewModels;

namespace webapp.Controllers;

public class ContactController: Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbcontext;

    public ContactController(ILogger logger, AppDbContext dbcontext)
    {
        _logger=logger;
        _dbcontext=dbcontext;
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
    try
    {
        var contacts= await _dbcontext.Contacts.ToListAsync();
        return View(contacts);
    }
    catch(Exception e)
    {
        return BadRequest(e.Message);
    }

    }
    [HttpGet]
    public async Task<IActionResult> GetContact(Guid id)
    {
       try
        {
            var contact= await _dbcontext.Contacts.FirstOrDefaultAsync(p=>p.Id==id);
            return View(contact);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult>Contact(ContactViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            var obj=new Contact()
            {
              Id=model.Id,
              Name=model.Name,
              Address=model.Address,
              Email=model.Email,
              Phone=model.Phone,
              Owner=model.Owner,
              OwnerId=model.OwnerId
            };
            await _dbcontext.Contacts.AddAsync(obj);
            return View();
        }
        catch(Exception e)
        {
            return View(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult>EditContact(Guid id,ContactViewModel model)
    {

        try
        {
          var contact= await _dbcontext.Contacts.FirstOrDefaultAsync(o=>o.Id==id);
            if(contact==null)
            {
                return NotFound();
            }
          contact.Id=model.Id;
          contact.Address=model.Address;
          contact.Email=model.Email;
          contact.Name=model.Name;
          contact.Owner=model.Owner;
          contact.Phone=model.Phone;
          contact.OwnerId=model.OwnerId;
         _dbcontext.Update(contact);
         await _dbcontext.SaveChangesAsync();
         return View();
        }
        catch(Exception e)
        {
            return View(e.Message);
        }
    } 
    [HttpGet]
    public async Task<IActionResult>DeleteContact(Guid id)
    {
        var item = await _dbcontext.Contacts.FirstOrDefaultAsync(y=>y.Id==id);
        if(item==null)
        {
            return NotFound();
        }
        _dbcontext.Remove(item);
       await  _dbcontext.SaveChangesAsync();
       return View();
    }
}