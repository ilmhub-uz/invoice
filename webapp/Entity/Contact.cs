using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapp.ViewModel;

namespace webapp.Entity;

public class Contact
{
   [Key]
   public Guid Id { get; set; } = Guid.NewGuid();  

   [Required]
   [Column(TypeName = "nvarchar(50)")] 
   public string Name { get; set; }  

   [Required]
   [Column(TypeName = "nvarchar(256)")]  
   public string Address { get; set; }  

   [Required]
   [Column(TypeName = "nvarchar(15)")] 
   public string Phone { get; set; }   

   [Required]
   [Column(TypeName = "nvarchar(128)")] 
   public string Email { get; set; } 

   public Guid OwnerId { get; set; }   
   public virtual AppUser Owner { get; set; }  
   public Guid OrganizationId { get; set; }
   public virtual Organization Organization { get; set; }

   public ContactViewModel Tomodel(Contact contact)
   {
        var model=new ContactViewModel(){
          Address=contact.Address,
          Email=contact.Email,
          Name=contact.Name,
          Owner=contact.Owner,
          Phone=contact.Phone,
          Organization=contact.Organization
         };
         return model;
   }
}
