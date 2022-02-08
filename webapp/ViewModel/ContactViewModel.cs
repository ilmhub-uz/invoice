namespace webapp.ViewModels;

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

   public Guid OwnerId { get; set; }   
   public virtual AppUser Owner { get; set; }  
}