using webapp.Entity;
using webapp.ViewModel;

namespace webapp.Extensions
{
    public static class OrganizationExtensions
    {        
        public static OrganizationViewModel ToOrganizationViewModel(this Organization entity)
        {
            return new OrganizationViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Address = entity.Address,
                Phone = entity.Phone,
                Email = entity.Email
            };
        }


        public static Entity.Organization ToOrganizationEntity(this CreateOrganizationViewModel model,Guid Id)
        {
            return new Entity.Organization(
        
                name : model.Name,
                address : model.Address,
                phone : model.Phone,
                email : model.Email,
                ownerId : Id
            );
        }
        
    }
}