using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Entity;

public class Contact
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Email { get; set; }

    public Guid OwnerId { get; set; }

    public virtual AppUser Owner { get; set; }
}