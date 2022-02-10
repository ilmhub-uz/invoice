namespace webapp.ViewModel
{
    public class ContactListViewModel
    {
        public int Page { get; set; }

        public int  Limit { get; set; }
        
        public int Pages { get; set; }

        public int TotalContacts { get; set; }
        
        public List<ContactViewModel> Contacts { get; set; }
          
    }
}