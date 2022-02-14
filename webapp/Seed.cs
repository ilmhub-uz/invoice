using webapp.Data;
using webapp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using webapp.Options;

public class Seed : BackgroundService
{

    private UserManager<AppUser> _userM;
    private RoleManager<IdentityRole<Guid>> _roleM;
    private readonly IServiceProvider _provider;
    private readonly ILogger<Seed> _logger;
    private readonly SeedDataOptions _options;

    public Seed(
        IServiceProvider provider,
        ILogger<Seed> logger,
        IConfiguration conf,
        IOptionsMonitor<SeedDataOptions> optionsMonitor)
    {
        _provider = provider;
        _logger = logger;
        _options = optionsMonitor.CurrentValue;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        _userM = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        _roleM = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var migrateDb = conf.GetValue<bool>("MigrateDatabase");

        var seedData = conf.GetValue<bool>("SeedData");
        
        _logger.LogInformation($"Database Migration option: {migrateDb}");
        if(migrateDb)
        {
            _logger.LogInformation($"Migrating Database...");
            ctx.Database.Migrate();
        }

        _logger.LogInformation($"Seed Data option: {_options.HasToSeed}");
        if(!_options.HasToSeed)
        {
            _logger.LogInformation("Seeding database is disabled. Cancelling Seed.cs service...");
            return;
        }
            _logger.LogInformation($"Seed Data...");
            
        foreach(var role in _options.Roles)
        {
            if(!await _roleM.RoleExistsAsync(role))
            {
                await _roleM.CreateAsync(new IdentityRole<Guid>(role));
                    _logger.LogInformation($"Create Role...");
            }
        }

        foreach (var user in _options.Users)
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

                if(user.Roles.Count() < 1)
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
                // ownerId = (newUser.Id);
        var OrganizationOption = conf.GetValue<OrganizationOption>("OrganizationOption");
        var owner = await _userM.FindByEmailAsync(OrganizationOption.Email);
        await ctx.Organizations.AddAsync(new Organization()
        {
            Id = Guid.NewGuid(),
            Name = OrganizationOption.Name,
            Address = OrganizationOption.Address,
            Email = OrganizationOption.Email,
            Phone = OrganizationOption.Phone,
            OwnerId = owner.Id,
            Owner = newUser
            
        });
        await ctx.SaveChangesAsync();

        await ctx.Contacts.AddAsync(new Contact()
        {
            Id = Guid.NewGuid(),
            Name = OrganizationOption.Name,
            Address = OrganizationOption.Address,
            Email = OrganizationOption.Email,
            Phone = OrganizationOption.Phone,
        });

        await ctx.SaveChangesAsync();
            }
        }
    }
}