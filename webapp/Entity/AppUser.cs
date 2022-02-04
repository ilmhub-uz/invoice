using Microsoft.AspNetCore.Identity;

namespace webapp.Entity;

public class AppUser : IdentityUser<Guid>
{
    public string Fullname { get; set; }
}