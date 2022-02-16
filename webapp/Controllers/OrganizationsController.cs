using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Entity;
using webapp.Extensions;
using webapp.ViewModel;
using webapp.ViewModels;

namespace webapp.Controllers;

   
[Authorize]
public class OrganizationsController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _ctx;
    private readonly UserManager<AppUser> _userm;

    public OrganizationsController(
                    AppDbContext context,
                    UserManager<AppUser> usermanager,
                    ILogger<OrganizationsController> logger)
    {
        
        _ctx = context;
        _userm = usermanager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
        
    public async Task<IActionResult> List(int page = 1, int limit = 10)
    {
        var user = await _userm.GetUserAsync(User);
        if(user == null)
        {
            return Unauthorized();
        }

        var existingOrgs = await _ctx.Organizations
        .Skip((page - 1) * limit)
        .Take(limit)
        .Select(u => new OrganizationViewModel
        {
            Id = u.Id,
            Name = u.Name,
            Phone = u.Phone,
            Email = u.Email,
            Address = u.Address,
        }).ToListAsync();
        
        var totalOrganizations = existingOrgs.Count();
        
        return View(new PaginatedListViewModel<OrganizationViewModel>()
        {
            Items = existingOrgs,
            TotalItems = (uint)totalOrganizations,
            TotalPages = (uint)Math.Ceiling(totalOrganizations / (double)limit),
            CurrentPage = (uint)page,
            Limit = (uint)limit
        });
    }   

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrganizationViewModel model)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogInformation($"Model validation failed for {JsonSerializer.Serialize(model)}");
            // return BadRequest("Organization properties are not valid.");
            return View();
        }

        var user = await _userm.GetUserAsync(User);

        var org = new Organization()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Address = model.Address,
            Phone = model.Phone,
            Email = model.Email,
            OwnerId = user.Id
        };

        try
        {
            await _ctx.Organizations.AddAsync(org);           
            await _ctx.SaveChangesAsync();
            _logger.LogInformation($"New organization added with ID: {org.Id}");
            _logger.LogInformation($"New EmployeeOrganization added with ID: {user.Id}");

            return RedirectToAction(nameof(Created));
            
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Error occured while creating organization:\n{e.Message}");
            return StatusCode(500, new { errorMessage = e.Message });
        }        
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var existingOrg = await _ctx.Organizations.FirstOrDefaultAsync(o => o.Id == id);
        if(existingOrg != default)
        {
            return View(existingOrg.ToOrganizationViewModel());
        }
        else
        {
            return RedirectToAction(nameof(Created));
        }
    }

    [HttpPost("{id}/update")]
    public async Task<IActionResult> UpdateModel(Guid id, UpdateOrganizationViewModel model)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogInformation($"Model validation failed for {JsonSerializer.Serialize(model)}");
            return RedirectToAction("Created");
        }
       
        var existingOrg = await _ctx.Organizations.FirstOrDefaultAsync(o => o.Id == id);
        if(existingOrg == null)
        {
            return NotFound();
        }        
        
        try
        {
            existingOrg.Address = model.Address;
            existingOrg.Phone = model.Phone;
            existingOrg.Email = model.Email; 
            existingOrg.Name = model.Name; 
                    
            await _ctx.SaveChangesAsync();  

            _logger.LogInformation($"Organization updated with ID: {existingOrg.Id}");  
            
            return RedirectToAction(nameof(Created));
            
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Error occured while updating organization:\n{e.Message}");
            return StatusCode(500, new { errorMessage = e.Message });
        }
    }

    [HttpPost("{id}/delete")]
    public async Task <IActionResult> Delete(Guid id)
    {
        if (!await _ctx.Organizations.AnyAsync(p => p.Id == id))
        {
            return NotFound();
        }

        var org = await _ctx.Organizations.FirstOrDefaultAsync(y => y.Id == id);

        try
        {
           _ctx.Organizations.Remove(org);

            await _ctx.SaveChangesAsync();

            _logger.LogInformation($"Organization deleted with ID: {org.Id}");

            return RedirectToAction("list");            
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Error occured while updating organization:\n{e.Message}");
            return StatusCode(500, new { errorMessage = e.Message });
        }
    }

    [HttpGet]
    public IActionResult New() => View(new CreateOrganizationViewModel());
   
    [HttpGet("{id}/show")]
    public async Task <IActionResult> Show(Guid id)
    {
        if (!await _ctx.Organizations.AnyAsync(c => c.Id == id))
        {
            return NotFound();
        }

        var org = await _ctx.Organizations.FirstOrDefaultAsync(p => p.Id == id);
        if (org == default)
        {
            return NotFound();
        }

        return View(org.ToOrganizationViewModel());
    }    
}
