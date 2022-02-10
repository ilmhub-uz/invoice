using webapp.Entity;
using webapp.ViewModel;

namespace webapp.Extensions;
public static class ToViewModels
{
    public static ContactViewModel ToContactViewModel(Contact entity)
    {
        return new ContactViewModel(){
          Id = entity.Id,
          Name=entity.Name,
          Address=entity.Address,
          Email=entity.Email,
          Organization=entity.Organization,
          Owner=entity.Owner,
          Phone=entity.Phone
        };
    }

    public static EditContactViewModel ToEditContactViewModel(Contact entity)
    {
        return new EditContactViewModel(){
          Id = entity.Id,
          Name=entity.Name,
          Address=entity.Address,
          Email=entity.Email,
          Organization=entity.Organization,
          Owner=entity.Owner,
          Phone=entity.Phone
        };
    }

    public static NewContactViewModel ToNewContactViewModel(Contact entity)
    {
        return new NewContactViewModel(){
          Id = entity.Id,
          Name=entity.Name,
          Address=entity.Address,
          Email=entity.Email,
          Organization=entity.Organization,
          Owner=entity.Owner,
          Phone=entity.Phone
        };
    }
}