using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Entity;

public class Invoice
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [Column(TypeName = "nvarchar(30)")] 
    public string Number { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(255)")] 
    public string Subject { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(255)")] 
    public string ToEmail { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    [Required]
    public DateTimeOffset ModifiedAt { get; set; }

    [Required]
    public DateTimeOffset PaymentDueAt { get; set; }

    [Required]
    public DateTimeOffset DeliveryDueAt { get; set; }
    public Guid OrganizationId { get; set; }
    public virtual Organization Organization { get; set; }
    public Guid BillToId { get; set; }
    public virtual Contact BillTo { get; set; }
    public virtual ICollection<InvoiceItem> Items { get; set; }
    public Guid? CreatorId { get; set; }
    public virtual AppUser Creator { get; set; }
}