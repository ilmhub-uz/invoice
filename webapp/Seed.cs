using webapp.Data;
using webapp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Seed : BackgroundService
{

    private UserManager<AppUser> _userM;
    private RoleManager<IdentityRole<Guid>> _roleM;
    private readonly IServiceProvider _provider;
    private readonly ILogger<Seed> _logger;
    private readonly InitialData _initialData;

    public Seed(
        IServiceProvider provider,
        ILogger<Seed> logger,
        IConfiguration conf)
    {
        _provider = provider;
        _logger = logger;
        _initialData = conf.GetSection("InitialData").Get<InitialData>();
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        _userM = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        _roleM = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var migrateDb = conf.GetValue<bool>("MigrateDatabase");
        var roles = _initialData.Roles.ToList();
        var SeedOrganization = _initialData.SeedOrganization;
        Guid ownerId = default(Guid);

        var seedData = conf.GetValue<bool>("SeedData");
        
        _logger.LogInformation($"Database Migration option: {migrateDb}");
        if(migrateDb)
        {
            
            _logger.LogInformation($"Migrating Database...");
            ctx.Database.Migrate();
        }

        _logger.LogInformation($"Seed Data option: {seedData}");
        if(seedData)
        {
            _logger.LogInformation($"Seed Data...");
            
            // add roles first
            foreach(var role in roles)
            {
                if(!await _roleM.RoleExistsAsync(role))
                {
                    await _roleM.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            foreach (var user in _initialData.Users)
            {
                if((await _userM.FindByEmailAsync(user.Email)) == null)
                {
                    var newUser = new AppUser()
                    {
                        Email = user.Email,
                        UserName = user.Username,
                        Fullname = user.Fullname,
                        PhoneNumber = user.Phone
                    };

                    var creationResult = await _userM.CreateAsync(newUser, user.Password);
                    
                    if(creationResult.Succeeded)
                    {
                        _logger.LogInformation($"{user.Email} has been added");
                    }
                    else
                    {
                        _logger.LogCritical($"{user.Email} has been failed to be created");
                    }

                    if(user.Roles.Count < 1)
                    {
                        foreach (var role in user.Roles)
                        {
                            var roleResponse = await _userM.AddToRoleAsync(newUser, role);
                            if(roleResponse.Succeeded)
                            {
                                _logger.LogInformation($"{user.Email} has been successfully added to role {role}");
                            }
                            else
                            {
                                _logger.LogCritical($"{user.Email} has been failed to be added to role {role}");
                            }
                        }
                    }
                    if(user.Roles.Contains("admin"))
                    {
                        ownerId = (newUser.Id);
                    }
                }
            }
            var organization = new Organization()
            {
                Id = SeedOrganization.Id,
                Name = SeedOrganization.Name,
                Address = SeedOrganization.Address,
                Email = SeedOrganization.Email,
                Phone = SeedOrganization.Phone,
                OwnerId = ownerId
            };
            await ctx.Organizations.AddAsync(organization);
            await ctx.SaveChangesAsync();
        }
    }
}

public class InitialData
{
    public bool SeedDatabase { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public List<SeedUser> Users { get; set; }

    public SeedOrganization SeedOrganization { get; set; }
}

public class SeedOrganization
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    public string Address { get; set; }
}

public class SeedUser
{
    public string Fullname { get; set; }
    
    public string Email { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public List<string> Roles { get; set; }

    public string Phone { get; set; }   
}

