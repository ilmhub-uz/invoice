using webapp.Entity;
using webapp.ViewModels;

namespace webapp.Extensions
{
    public static class OrganizationExtensions
    {
        public static OrganizationViewModel ToOrganizationModel(this Organization entity)
        {
            return new OrganizationViewModel()
            {
                Id = entity.Id,
                Name = entity.Name                
            };
        }
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
        
    }
}