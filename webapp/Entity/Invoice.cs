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

    public Guid FromId { get; set; }
    public virtual Organization From { get; set; }

    public Guid BillToId { get; set; }
    public virtual Contact BillTo { get; set; }

    public Guid OwnerId { get; set; }
    public virtual AppUser Owner { get; set; }

    public virtual ICollection<InvoiceItem> Items { get; set; }

    [Obsolete("", true)]
    public Invoice() { }

    public Invoice(string number, string subject, string toEmail, Guid fromId, Guid billToId, Guid ownerId)
    {
        Id = Guid.NewGuid();
        Number = number;
        Subject = subject;
        ToEmail = toEmail;
        CreatedAt = DateTimeOffset.UtcNow;
        ModifiedAt = default(DateTimeOffset);
        PaymentDueAt = default(DateTimeOffset);
        DeliveryDueAt = default(DateTimeOffset);
        FromId = fromId;
        BillToId = billToId;
        OwnerId = ownerId;
    }
}