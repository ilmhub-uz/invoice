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
                await _roleM.CreateAsync(new IdentityRole<Guid>(role.ToLower()));
                _logger.LogInformation("Created role ", role.ToUpper());
            }
        }

        foreach (var user in _options.Users)
        {
            if((await _userM.Users.AnyAsync(u => u.Email == user.Email)))
            {
                _logger.LogInformation("User already exists so not seeding: ", user.Email);
                continue;
            }

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
                _logger.LogInformation("User seeding succeded", user.Email);
            }
            else
            {
                _logger.LogCritical("User seeding failed", user.Email);
            }

            if(user.Roles?.Count() > 0)
            {
                foreach (var role in user.Roles)
                {
                    if(await _roleM.RoleExistsAsync(role.ToLower()) && !await _userM.IsInRoleAsync(newUser, role.ToLower()))
                    {
                        var roleResponse = await _userM.AddToRoleAsync(newUser, role);
                    }
                }
            }
        }


        foreach(var org in _options.Organizations)
        {
            var owner = await _userM.FindByEmailAsync(org.OwnerEmail);
            if(owner == null) continue;

            if(await ctx.Organizations.AnyAsync(o => o.Email == org.Email))
            {
                continue;
            }

            Organization newOrg = new()
            {
                Id = Guid.NewGuid(),
                Name = org.Name,
                Email = org.Email,
                Address = org.Address,
                Phone = org.Phone,
                OwnerId = owner.Id
            };

            await ctx.Organizations.AddAsync(newOrg);
            await ctx.SaveChangesAsync();

        }
    }
}