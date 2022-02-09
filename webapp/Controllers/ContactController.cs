using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Entity;
using webapp.ViewModel;

namespace webapp.Controllers;

public class ContactController: Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbcontext;

    public ContactController(ILogger<ContactController> logger, AppDbContext dbcontext)
    {
        _logger=logger;
        _dbcontext=dbcontext;
    }

    [HttpGet]
    [Route("contacts")]
    public IActionResult Index()
    {
       return View();
    }
    [HttpGet]
    [Route("contacts/list")]
    public async Task<IActionResult>List()
    {
    try
    {
        var contacts = await _dbcontext.Contacts.ToListAsync();
        if(contacts==null)
        {
            return NotFound();
        }
        var contactlist=new List<ContactViewModel>();

        foreach(var model in contacts){
            var parsed = model.Tomodel(model);
            contactlist.Add(parsed);
        }
        return View(contactlist);
    }
    catch(Exception e)
    {
        return BadRequest(e.Message);
    }
    }
    [HttpGet]
    [Route("contacts/new")]
    public IActionResult New()
    {
        ContactViewModel model=new ContactViewModel();
        return View(model);
    }

    [HttpGet]
    [Route("contacts/{id}/show")]
    public async Task<IActionResult> Show(Guid id)
    {
       try
        {
            var contact= await _dbcontext.Contacts.FirstOrDefaultAsync(p=>p.Id==id);
            var res = contact.Tomodel(contact);
            return View(res);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet]
    [Route("contacts/{id}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
       try
        {
            var contact= await _dbcontext.Contacts.FirstOrDefaultAsync(p=>p.Id==id);
            var res = contact.Tomodel(contact);
            return View(res);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("contacts/create")]
    public async Task<IActionResult>Create(ContactViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            var obj=model.ToEntity(model);

            await _dbcontext.Contacts.AddAsync(obj);
            await _dbcontext.SaveChangesAsync();
            
            var result = obj.Tomodel(obj);
            return RedirectToAction("show",result.Id);
        }
        catch(Exception e)
        {
            return View(e.Message);
        }
    }

    [HttpPost]
    [Route("/contacts/{id}/update")]
    public async Task<IActionResult> Update(Guid id,ContactViewModel model)
    {
        try
        {
          var contact= await _dbcontext.Contacts.FirstOrDefaultAsync(o=>o.Id==id);
            if(contact==null)
            {
                return NotFound();
            }
          contact.Address=model.Address;
          contact.Email=model.Email;
          contact.Name=model.Name;
          contact.Owner=model.Owner;
          contact.Phone=model.Phone;

         _dbcontext.Contacts.Update(contact);
         await _dbcontext.SaveChangesAsync();

         var result=contact.Tomodel(contact);
         return RedirectToAction("show",result.Id);
        }
        catch(Exception e)
        {
            return View(e.Message);
        }
    } 
    [HttpPost]
    [Route("/contacts/{id}/delete")]
    public async Task<IActionResult>Delete(Guid id)
    {
        var item = await _dbcontext.Contacts.FirstOrDefaultAsync(y=>y.Id==id);
        if(item==null)
        {
            return NotFound();
        }
        _dbcontext.Remove(item);
       await  _dbcontext.SaveChangesAsync();
       return RedirectToAction("list");
    }
}