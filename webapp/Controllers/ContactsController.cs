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
    public async Task<IActionResult> List(int page = 1, int limit = 10)
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
        }).ToListAsync();
        if (contacts == null)
        {
            return NotFound();
        }

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

    [HttpGet]
    public IActionResult New()
    {
        return View(new NewContactViewModel());
    }

    [HttpGet("{id}/show")]
    public async Task<IActionResult> Show(Guid id)
    {
        if (!await _dbcontext.Contacts.AnyAsync(c => c.Id == id))
        {
            return NotFound();
        }

        var contact = await _dbcontext.Contacts.FirstOrDefaultAsync(p => p.Id == id);
        if (contact == default)
        {
            return NotFound();
        }

        return View(contact.ToContactViewModel());
    }

    [HttpGet("{id}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!await _dbcontext.Contacts.AnyAsync(c => c.Id == id))
        {
            return NotFound();
        }

        var contact = await _dbcontext.Contacts.FirstOrDefaultAsync(p => p.Id == id);
        if (contact == default)
        {
            return NotFound();
        }
        return View(contact.ToEditContactViewModel());

    }

    [HttpPost]
    public async Task<IActionResult> Create(NewContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = model.ToContactEntity();
        await _dbcontext.Contacts.AddAsync(result);

        return RedirectToAction("show", result.Id);
    }

    [HttpPost("{id}/update")]
    public async Task<IActionResult> Update(Guid id, EditContactViewModel model)
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

        _dbcontext.Contacts.Update(contact);
        await _dbcontext.SaveChangesAsync();

        var result = contact.ToEditContactViewModel();

        return RedirectToAction("show", result.Id);

    }
    [HttpPost("{id}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _dbcontext.Contacts.AnyAsync(p => p.Id == id))
        {
            return NotFound();
        }

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