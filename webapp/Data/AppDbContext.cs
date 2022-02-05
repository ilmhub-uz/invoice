using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapp.Entity;

namespace webapp.Data;
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }
    public DbSet<Organization> Organizations{ get; set;}
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppUser>()
                .HasMany(u => u.Organizations)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
                // .HasPrincipalKey(u => u.Organizations);
    }
}