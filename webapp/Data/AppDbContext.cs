using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapp.Entity;

namespace webapp.Data;
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Contact> Contacts { get => Contacts; set => Contacts = value; }
    public AppDbContext(DbSet<Contact> contacts)
    {
        this.Contacts = contacts;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
                .HasMany(u => u.Organizations)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
