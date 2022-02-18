using webapp.Entity;
using webapp.ViewModel;

namespace webapp.Extensions;
public static class ToEntities
{
   public static Contact ToContactEntity( this NewContactViewModel model)
   {
       return new Contact(
            model.Name,
            model.Address, 
            model.Phone, 
            model.Email, 
            model.Owner.Id);
   }
}