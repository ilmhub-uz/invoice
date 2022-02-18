using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Entity;

public class Organization
{
   [Key]
   public Guid Id { get; set; }   

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
   
   public virtual ICollection<Invoice> Invoices { get; set; }

   [Obsolete("Used only for entity binding.", true)]
    public Organization() { }

    public Organization(string name, string address, string phone, string email, Guid ownerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        OwnerId = ownerId;
    }
}