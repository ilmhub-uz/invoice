using webapp.ViewModels;

namespace webapp.ViewModel
{
    public class OrganizationsListViewModel
    {
        public int Page { get; set; }

        public int  Limit { get; set; }
        
        public int totalPages { get; set; }

        public int totalOrganizationsCount { get; set; }
        
        public List<OrganizationViewModel> Organizations { get; set; }
        
    }
}