using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Extensions;
using webapp.ViewModel;

namespace webapp.Controllers;

public class ContactsController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbcontext;

    public ContactsController(ILogger<ContactsController> logger, AppDbContext dbcontext)
    {
        _logger = logger;
        _dbcontext = dbcontext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> List(string query, int page = 1, int limit = 10)
    {
        try
        {
            var contacts = await _dbcontext.Contacts
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(u => new ContactViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Phone = u.Phone,
                Email = u.Email,
                Owner = u.Owner,
                Organization = u.Organization
            }).ToListAsync();


            var totalContacts = contacts.Count();

            return View(new ContactListViewModel()
            {
                Contacts = contacts,
                TotalContacts = totalContacts,
                Pages = (int)Math.Ceiling(totalContacts / (double)limit),
                Page = page,
                Limit = limit
            });

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public IActionResult New()
    {
      return View(new NewContactViewModel());
    }

    [HttpGet("{id}/show")]
    public async Task<IActionResult> Show(Guid id)
    {
        try
        {
            var contact = await _dbcontext.Contacts.FirstOrDefaultAsync(p => p.Id == id);
            var result = ToViewModels.ToContactViewModel(contact);
            return View(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var contact = await _dbcontext.Contacts.FirstOrDefaultAsync(p => p.Id == id);
            var result = ToViewModels.ToEditContactViewModel(contact);
            return View(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            var result = ToEntities.ToContactEntity(model);
            await _dbcontext.Contacts.AddAsync(result);

            return RedirectToAction("show",result.Id);
        }
        catch (Exception e)
        {
            return View(e.Message);
        }
    }

    [HttpPost("{id}/update")]
    public async Task<IActionResult> Update(Guid id, EditContactViewModel model)
    {
        try
        {
            var contact = await _dbcontext.Contacts.FirstOrDefaultAsync(o => o.Id == id);
            if (contact == default)
            {
                return NotFound();
            }
            contact.Address = model.Address;
            contact.Email = model.Email;
            contact.Name = model.Name;
            contact.Owner = model.Owner;
            contact.Phone = model.Phone;
            contact.Organization = model.Organization;

            _dbcontext.Contacts.Update(contact);
            await _dbcontext.SaveChangesAsync();
            
            var result = ToViewModels.ToEditContactViewModel(contact);

            return RedirectToAction("show",result.Id);
        }
        catch (Exception e)
        {
            return View(e.Message);
        }
    }
    [HttpPost("{id}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _dbcontext.Contacts.FirstOrDefaultAsync(y => y.Id == id);
        if (item == default)
        {
            return NotFound();
        }
        _dbcontext.Remove(item);
        await _dbcontext.SaveChangesAsync();
        return RedirectToAction("list");
    }
}