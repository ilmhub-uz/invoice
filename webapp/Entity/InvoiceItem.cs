using System.ComponentModel.DataAnnotations;

namespace webapp.Entity;
    public class InvoiceItem
    {
      [Key]  
      public Guid Id { get; set; }
      
      [Required]
      [MaxLength(255)]
      public string Name { get; set; }

      [Required]
      public int Quantity { get; set; }

      [Required]
      public double Rate { get; set; }

      [Required]
      public string Currency { get; set; }
      public Guid InvoiceId { get; set; }
      public Invoice Invoice { get; set; }
      
        
    }
