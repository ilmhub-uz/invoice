using webapp.Entity;
using webapp.ViewModel;

namespace webapp.Extensions;
public static class ToEntities
{
   public static Contact ToContactEntity( this NewContactViewModel model)
   {
       return new Contact(){
        Id=model.Id,
        Name=model.Name,
        Address=model.Address,
        Email=model.Email,
        Phone=model.Phone,
        Owner=model.Owner
       };
   }
}