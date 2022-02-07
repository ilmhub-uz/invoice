using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace webapp.Entity;

public class Invoice : ValidationAttribute
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MinLength(32)]
    public string Number { get; set; }

    [Required]
    [MaxLength(255)]
    public string Subject { get; set; }

    [Required]
    [MaxLength(255)]
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
    public virtual Organization Organization { get; set; }

    [Required]
    public Guid ToId { get; set; }
    public virtual Contact BillTo { get; set; }
    public virtual ICollection<InvoiceItem> Items { get; set; }
    public Guid CreatorId { get; set; }
    public virtual AppUser Creator { get; set; }
    public override bool IsValid(object value)
    {
        DateTimeOffset dateTime;
        var isValid = DateTimeOffset.TryParseExact
        (
        Convert.ToString(value),
        "dd mmm yyyy",
        CultureInfo.CurrentCulture,
        DateTimeStyles.None,
        out dateTime
        );

        return (isValid && dateTime > DateTimeOffset.UtcNow);
    }

}