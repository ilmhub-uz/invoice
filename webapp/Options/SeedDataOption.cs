namespace webapp.Options;

public class SeedDataOptions
{
    public bool HasToSeed { get; set; }
    public List<string> Roles { get; set; }
    public List<UserOption> Users { get; set; }
    public List<OrganizationOption> Organizations { get; set; }
    public List<ContactOption> Contacts { get; set; }
}
    
