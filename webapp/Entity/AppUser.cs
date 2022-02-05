using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace webapp.Entity;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; }

    [ForeignKey("Organization")]
    public virtual ICollection<Organization> Organizations { get; set; }
}