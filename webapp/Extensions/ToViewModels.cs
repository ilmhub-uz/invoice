using webapp.Entity;
using webapp.ViewModel;
using webapp.ViewModels;

namespace webapp.Extensions;
public static class ToViewModels
{
    public static ContactViewModel ToContactViewModel(this Contact entity)
    {
        return new ContactViewModel(){
          Id = entity.Id,
          Name=entity.Name,
          Address=entity.Address,
          Email=entity.Email,
          Owner=entity.Owner,
          Phone=entity.Phone
        };
    }

    public static EditContactViewModel ToEditContactViewModel(this Contact entity)
    {
        return new EditContactViewModel(){
          Id = entity.Id,
          Name=entity.Name,
          Address=entity.Address,
          Email=entity.Email,
          Owner=entity.Owner,
          Phone=entity.Phone
        };
    }

    public static NewContactViewModel ToNewContactViewModel(this Contact entity)
    {
        return new NewContactViewModel(){
          Id = entity.Id,
          Name=entity.Name,
          Address=entity.Address,
          Email=entity.Email,
          Owner=entity.Owner,
          Phone=entity.Phone
        };
    }




    public static CreatedOrganizationsViewModel ToModel(this List<Organization> entity)
    {
      
        return new CreatedOrganizationsViewModel()
        {
            Organizations = entity.Select(i => 
            {
                return new OrganizationViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    // InvoicesCount = i.Invoices.Count(),
                    // ContactsCount = i.Contacts.Count()
                };
            }).ToList()
        };
    }

    public static OrganizationViewModel ToOrganizationModel(this Organization entity)
    {
        AppUser user = entity.Owner;
        return new OrganizationViewModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            ContactsCount = user.Contacts.Count(),
            InvoicesCount = entity.Invoices.Count()
        };
    }
    public static OrganizationModel ToOrgModel(this Organization entity)
    {
        return new OrganizationModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
            Phone = entity.Phone,
            Email = entity.Email
        };
    }
}