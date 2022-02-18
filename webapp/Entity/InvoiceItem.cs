using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapp.Entity;
public class InvoiceItem
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(255)")]

    public string Name { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public double Rate { get; set; }

    [Required]
    public string Currency { get; set; }

    public Guid InvoiceId { get; set; }

    public virtual Invoice Invoice { get; set; }

    [Obsolete("", true)]
    public InvoiceItem() { }

    public InvoiceItem(string name, int quantity, double rate, string currency, Guid invoiceId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Quantity = quantity;
        Rate = rate;
        Currency = currency;
        InvoiceId = invoiceId;
    }
}
