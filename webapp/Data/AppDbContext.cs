using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapp.Entity;

namespace webapp.Data;
public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> IvoiceItems { get; set; }
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
            au.HasMany(u => u.Invoices)
                .WithOne(i => i.Creator)
                .HasForeignKey(i => i.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Organization>(o =>
       {

         o.HasMany(org => org.Contacts)
             .WithOne(p => p.Organization)
             .HasForeignKey(p => p.OrganizationId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);
       });

        builder.Entity<Organization>(o =>
       {
           o.HasMany(org => org.Invoices)
               .WithOne(i => i.Organization)
               .HasForeignKey(i => i.OrganizationId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
       });

        builder.Entity<Invoice>(i =>
        {
            i.HasMany(inv => inv.Items)
                .WithOne(item => item.Invoice)
                .HasForeignKey(item => item.InvoiceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            i.HasIndex(inv => new { inv.OrganizationId, inv.Number })
                .IsUnique();
        });
    }
}