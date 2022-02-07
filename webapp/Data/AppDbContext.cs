using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapp.Entity;

namespace webapp.Data;
public class AppDbContext : IdentityDbContext
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppUser>(au => 
        {
            au.HasMany(u => u.Organizations)
                .WithOne(i => i.Owner)
                .HasForeignKey(i => i.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<AppUser>(au => 
        {
            au.HasMany(u => u.Contacts)
                .WithOne(i => i.Owner)
                .HasForeignKey(i => i.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}