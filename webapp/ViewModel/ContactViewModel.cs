using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapp.Entity;

namespace webapp.ViewModel;

public class ContactViewModel
{
   [Key]
   public Guid Id { get; set; } 

   [Required(ErrorMessage = "ism-sharfini kiritish shart!")]
   [Display(Name = "Ism")]
   public string Name { get; set; }  

   [Required(ErrorMessage = "Manzilni kiriting")]
   [Display(Name = "Ism")]  
   public string Address { get; set; }  

   [Required]
   [Column(TypeName = "nvarchar(15)")] 
   public string Phone { get; set; }   

   [Required]
   [Column(TypeName = "nvarchar(128)")] 
   public string Email { get; set; } 
   public virtual AppUser Owner { get; set; }    

   public virtual Organization Organization { get; set; } 

   public  Contact ToEntity(ContactViewModel model)
   {
        var contact=new Contact(){
          Address=model.Address,
          Email=model.Email,
          Name=model.Name,
          Owner=model.Owner,
          Phone=model.Phone,
          Organization=model.Organization
         };
         return contact;
   }
}