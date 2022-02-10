namespace webapp.Controllers
{
   
[Authorize]
public class OrganizationController : Controller
{
    private readonly ILogger _logger;
    private readonly AppDbContext _ctx;
    private readonly UserManager<AppUser> _userm;

    public OrganizationController(AppDbContext context, UserManager<AppUser> usermanager,ILogger<OrganizationController> logger)
    {
        
        _ctx = context;
        _userm = usermanager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Created()
    {
        var user = await _userm.GetUserAsync(User);
        if(user == null)
        {
            return Unauthorized();
        }

        var orgs = await _ctx.Organizations.Where(o => o.OwnerId == user.Id).ToListAsync();
        
        var model = orgs.ToModel();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Joined()
    {
        var user = await _userm.GetUserAsync(User);
        if(user == null)
        {
            return Unauthorized();
        }
        
        var model = user.EmployeeOrganizations
            .Select(org => org.Organization)
            .Where(org => org.OwnerId != user.Id)
            .ToList().ToModel();
        
        return View(model);
    }
    

    [HttpGet]
    public IActionResult Create() => View();

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

            var empOrg = new EmployeeOrganization()
            {
                EmployeeId = user.Id,
                OrganizationId = org.Id
            };
            
            await _ctx.EmployeeOrganizations.AddAsync(empOrg);

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
    public IActionResult AddMember(Guid id) => View(new AddMemberViewModel() { OrganizationId = id });

    [HttpPost]
    public async Task<IActionResult> AddMember(AddMemberViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }

        if(await _ctx.JoinCodes.AnyAsync(c => c.ForEmail == model.ForEmail))
        {
            ModelState.AddModelError("ForEmail", "Bu emailga taklif jo'natilgan.");
            return View();
        }

        if(!await _ctx.Users.AnyAsync(u => u.Email == model.ForEmail))
        {
            ModelState.AddModelError("ForEmail", "Bu email allaqachon olingan.");
            return View();
        }
        else
        {
            // send invitation email
        }

        var user = await _userm.GetUserAsync(User);


        var joinCode = new JoinCode()
        {
            ForEmail = model.ForEmail,
            OrganizationId = model.OrganizationId,
            CreatorId = user.Id
        };

        await _ctx.JoinCodes.AddAsync(joinCode);
        await _ctx.SaveChangesAsync();

        
        return RedirectToAction(nameof(Created));
    }




    [HttpGet]
    public IActionResult Update(Guid id)
    {
        var returnedModel = _ctx.Organizations.FirstOrDefault(o => o.Id == id);
        if(returnedModel != default)
        {
            return View(returnedModel.ToOrgModel());
        }
        else
        {
            return RedirectToAction(nameof(Created));
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateModel(Guid id, OrganizationModel model)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogInformation($"Model validation failed for {JsonSerializer.Serialize(model)}");
            return RedirectToAction("Created");
        }
       
        var returnedModel = _ctx.Organizations.FirstOrDefault(o => o.Id == id);
        if(returnedModel == null)
        {
            return NotFound();
        }        
        
        try
        {
            returnedModel.Address = model.Address;
            returnedModel.Phone = model.Phone;
            returnedModel.Email = model.Email; 
            returnedModel.Name = model.Name; 
                    
            await _ctx.SaveChangesAsync();  
            _logger.LogInformation($"Organization updated with ID: {returnedModel.Id}");  
            return RedirectToAction(nameof(Created));
            
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Error occured while updating organization:\n{e.Message}");
            return StatusCode(500, new { errorMessage = e.Message });
        }
    }


    [HttpGet]
    public IActionResult Delete(Guid id)
    {
        var returnedModel = _ctx.Organizations.FirstOrDefault(o => o.Id == id);
        if(returnedModel != default)
        {
            return View(returnedModel.ToOrgModel());
        }
        else
        {
            return RedirectToAction(nameof(Created));
        }
    }

    [HttpPost]
    public IActionResult DeleteModel(Organization entity)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogInformation($"Model validation failed for {JsonSerializer.Serialize(entity)}");
            return RedirectToAction("Created");
        }

        var org = _ctx.Organizations.FirstOrDefault(o => o.Id == entity.Id);
        
        try
        {
            _ctx.Organizations.Remove(org);
            
            _ctx.SaveChangesAsync();
            _logger.LogInformation($"Organization deleted with ID: {org.Id}");
    
            return RedirectToAction(nameof(Created));
            
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Error occured while updating organization:\n{e.Message}");
            return StatusCode(500, new { errorMessage = e.Message });
        }
    }




    [HttpGet]
    public IActionResult GetById(OrganizationViewModel org)
    {
        return View();
    }


    [HttpPost]
    public IActionResult GetOrganizationById(OrganizationViewModel org)
    {
        if(!ModelState.IsValid)
        {
            return NotFound();
        }

        
        var returnedModel = _ctx.Organizations.FirstOrDefault(o => o.Id == org.Id);
        if(returnedModel != default)
        {
            return View("Get", returnedModel.ToOrganizationModel());
        }
        else
        {
            return RedirectToAction(nameof(Created));
        }
    
    }
    
}
}