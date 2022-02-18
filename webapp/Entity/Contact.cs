using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapp.ViewModel;

namespace webapp.Entity;

public class Contact
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

    [Obsolete("", true)]
    public Contact() { }

    public Contact(string name, string address, string phone, string email, Guid ownerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        OwnerId = ownerId;
    }
}
